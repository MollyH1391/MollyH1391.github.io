using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WowDin.Backstage.Models;
using WowDin.Backstage.Models.Dto.Menu;
using WowDin.Backstage.Models.Entity;
using WowDin.Backstage.Models.ViewModel.Menu;
using WowDin.Backstage.Repositories.Interface;
using WowDin.Backstage.Services.Interface;
using WowDin.Backstage.Common;
using WowDin.Backstage.Common.ModelEnum;

namespace WowDin.Backstage.Services
{
    public class MenuService : IMenuService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuService> _logger;
        public MenuService(IRepository repository, IMapper mapper, ILogger<MenuService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public APIResult GetShopsOfBrand(int brandId)
        {
            try
            {
                var shops = _repository.GetAll<Shop>().Where(x => x.BrandId == brandId && x.State != (int)ShopEnum.StateEnum.Remove);
                if (shops != null)
                {
                    var result = shops.Select(x => new { id = x.ShopId, name = x.Name }).ToList();
                    return new APIResult(APIStatus.Success, "店面讀取成功", result);
                }
                else
                {
                    return new APIResult(APIStatus.Success, "尚無店面資料", null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "店面讀取失敗");
                return new APIResult(APIStatus.Fail, "店面讀取失敗", null);
            }
        }
        public APIResult GetMenuData(int shopId)
        {
            var menu = new GetMenuDataDto();
            try
            {
                var menuList = _repository.GetAll<MenuClass>().Where(x => x.ShopId == shopId).OrderBy(x => x.Sort).ToList();
                var productList = _repository.GetAll<Product>().Where(x => menuList.Select(y => y.MenuClassId).Contains(x.MenuClassId)).OrderBy(x => x.Sort).ToList();
                var customList = _repository.GetAll<Custom>().Where(x => productList.Select(y => y.ProductId).Contains(x.ProductId)).ToList();
                var selectionList = _repository.GetAll<CustomSelection>().Where(x => customList.Select(y => y.CustomId).Contains(x.CustomId)).ToList();

                menu.MenuClasses = menuList.Select(x =>
                new MenuClassDto()
                {
                    Name = x.ClassName.Trim(),
                    Id = x.MenuClassId,
                    Products = productList.Where(p => p.MenuClassId == x.MenuClassId).Select(p => new ProductDto()
                    {
                        Name = p.Name.Trim(),
                        Id = p.ProductId,
                        BasicPrice = p.BasicPrice,
                        Figure = p.Fig,
                        State = ((ProductEnum.StateEnum)p.State).GetDescription(),
                        ChangeNote = p.Note,
                        Customs = customList.Where(c => c.ProductId == p.ProductId).Select(c => new CustomDto
                        {
                            Name = c.Name.Trim(),
                            Id = c.CustomId,
                            MaxAmount = c.MaxAmount,
                            Necessary = c.Necessary,
                            Selections = selectionList.Where(s => s.CustomId == c.CustomId).Select(s => new SelectionDto
                            {
                                Name = s.Name.Trim(),
                                Id = s.CustomSelectionId,
                                Price = s.AddPrice
                            })
                        })
                    })
                });

                return new APIResult(APIStatus.Success, "菜單資料讀取成功", menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "菜單資料讀取失敗");
                return new APIResult(APIStatus.Fail, "菜單資料讀取失敗", null);
            }
        }
        public APIResult CopyMenu(CopyMenuInputVM request)
        {
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var menuSource = request.Source;

                    foreach (var shopId in request.Targets)
                    {
                        var classesToDelete = _repository.GetAll<MenuClass>().Where(x => x.ShopId == shopId).ToList();
                        foreach (var menuClass in classesToDelete)
                        {
                            DeleteClassOperation(new ClassInputVM { ShopId = shopId, MenuClassId = menuClass.MenuClassId });
                        }

                        foreach (var menuClass in menuSource)
                        {
                            var classId = CreateClassOperation(new ClassInputVM { ShopId = shopId, ClassName = menuClass.Name });

                            foreach (var product in menuClass.Products)
                            {
                                var productInputVM = _mapper.Map<ProductInputVM>(product);
                                productInputVM.ShopId = shopId;
                                productInputVM.MenuClassId = classId;
                                var productId = CreateProductOperation(productInputVM);
                                foreach (var custom in product.Customs)
                                {
                                    CreateCustomOperation(custom, productId);
                                }
                            }
                        }
                        RecordChange(shopId, $"複製分類{String.Join('、', request.Source.Select(x => x.Id))}至菜單", Extensions.JsonSerialize(request.Source));
                    }

                    transaction.Commit();

                    return new APIResult(APIStatus.Success, "保存成功", null);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "菜單複製失敗");
                    return new APIResult(APIStatus.Fail, "保存失敗", null);
                }
            }
        }
        public APIResult CopyCustom(CopyCustomInputVM request)
        {
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var targetClass = _repository.GetAll<MenuClass>().Where(x => request.ClassIds.Contains(x.MenuClassId)).ToList();
                    var targetProducts = _repository.GetAll<Product>().Where(x => targetClass.Select(c => c.MenuClassId)
                                            .Contains(x.MenuClassId) && x.ProductId != request.SourceProductId).ToList();

                    var productSource = _repository.GetAll<Product>().First(x => x.ProductId == request.SourceProductId);
                    var customs = _repository.GetAll<Custom>().Where(x => x.ProductId == productSource.ProductId).ToList();
                    var selections = _repository.GetAll<CustomSelection>().Where(x => customs.Select(c => c.CustomId).Contains(x.CustomId)).ToList();

                    foreach (var product in targetProducts)
                    {

                        foreach (var custom in customs)
                        {
                            var customVM = new CustomVM
                            {
                                Name = custom.Name,
                                Necessary = custom.Necessary,
                                MaxAmount = custom.MaxAmount,
                                Selections = selections.Where(x => x.CustomId == custom.CustomId).OrderBy(x => x.CustomSelectionId).Select(x => new SelectionVM
                                {
                                    Name = x.Name,
                                    Price = x.AddPrice
                                })
                            };

                            CreateCustomOperation(customVM, product.ProductId);
                        }

                        RecordChange(targetClass[0].ShopId, $"由產品{productSource.Name}複製選項", Extensions.JsonSerialize(request));
                    }

                    transaction.Commit();

                    return new APIResult(APIStatus.Success, "保存成功", null);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "題型複製失敗");
                    return new APIResult(APIStatus.Fail, "保存失敗", null);
                }
            }

            
        }
        public DateTime GetRecord(int shopId)
        {
            try
            {
                var record = _repository.GetAll<MenuHistory>().Where(x => x.ShopId == shopId)
                                .OrderBy(x => x.UpdateTime).LastOrDefault();
                if (record != null)
                {
                    record.UpdateTime = record.UpdateTime.TransferToTaipeiTime();
                    return record.UpdateTime;
                }
                else
                {
                    var recordOfShop = _repository.GetAll<ShopHistory>().Where(x => x.ShopId == shopId)
                                        .OrderBy(x => x.UpdateDate).LastOrDefault();
                    return recordOfShop.UpdateDate.TransferToTaipeiTime();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "紀錄獲取失敗");
                return default;
            }
        }
        public APIResult SaveArrangement(ArrangementInputVM request)
        {
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var targetClasses = _repository.GetAll<MenuClass>().Where(x => x.ShopId == request.ShopId).ToList();
                    var targetProducts = _repository.GetAll<Product>().Where(x => targetClasses.Select(y => y.MenuClassId).Contains(x.MenuClassId)).ToList();

                    for (int i = 0; i < request.MenuClasses.Count(); i++)
                    {
                        var currentClass = targetClasses.First(x => x.MenuClassId == request.MenuClasses[i].Id);
                        currentClass.Sort = i + 1;

                        _repository.Update(currentClass);

                        for (int j = 0; j < request.MenuClasses[i].Products.Count(); j++)
                        {
                            var currentProduct = targetProducts.First(x => x.ProductId == request.MenuClasses[i].Products[j].Id);
                            currentProduct.MenuClassId = request.MenuClasses[i].Id;
                            currentProduct.Sort = j + 1;
                            _repository.Update(currentProduct);
                        }
                    }

                    _repository.Save();
                    transaction.Commit();

                    RecordChange(request.ShopId, $"更新排序", Extensions.JsonSerialize(request));
                    return new APIResult(APIStatus.Success, "保存成功", null);
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "排序保存失敗");
                    return new APIResult(APIStatus.Fail, "保存失敗", null);
                }
            }
        }
        public APIResult CreateClass(ClassInputVM request)
        {
            try
            {
                var classId = CreateClassOperation(request);
                RecordChange(request.ShopId, "建立類別", Extensions.JsonSerialize(request));

                return new APIResult(APIStatus.Success, "新增成功", classId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "類別建立失敗");
                return new APIResult(APIStatus.Fail, "新增失敗", false);
            }
        }
        public APIResult EditClass(ClassInputVM request)
        {
            try
            {
                var target = _repository.GetAll<MenuClass>().First(x => x.MenuClassId == request.MenuClassId);
                target.ClassName = request.ClassName.Trim();

                _repository.Update(target);
                _repository.Save();
                RecordChange(request.ShopId, "編輯類別", Extensions.JsonSerialize(request));

                return new APIResult(APIStatus.Success, "保存成功", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "類別更新失敗");
                return new APIResult(APIStatus.Fail, "保存失敗", false);
            }
        }
        public APIResult DeleteClass(ClassInputVM request)
        {
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var effectAmount = DeleteClassOperation(request);
                    RecordChange(request.ShopId, $"刪除類別，共{effectAmount}筆資料刪除", Extensions.JsonSerialize(request));
                    transaction.Commit();

                    return new APIResult(APIStatus.Success, "刪除成功", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "類別刪除失敗");
                    return new APIResult(APIStatus.Fail, "刪除失敗", false);
                }
            }
        }
        public APIResult TempDeleteProduct(ProductDeleteInputVM request)
        {
            try
            {
                var product = _repository.GetAll<Product>().First(x => x.ProductId == request.Id);
                product.State = (int)ProductEnum.StateEnum.Deleted;
                _repository.Save();
                RecordChange(request.ShopId, "產品丟到垃圾桶", Extensions.JsonSerialize(request));

                return new APIResult(APIStatus.Success, "刪除成功", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "產品刪除失敗");
                return new APIResult(APIStatus.Fail, "刪除失敗", false);
            }
        }
        public APIResult DeleteProduct(ProductDeleteInputVM request)
        {
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var product = _repository.GetAll<Product>().First(x => x.ProductId == request.Id);
                    var customs = _repository.GetAll<Custom>().Where(x => x.ProductId == product.ProductId);
                    if (customs != null)
                    {
                        foreach (var custom in customs.ToList())
                        {
                            var selections = _repository.GetAll<CustomSelection>().Where(x => x.CustomId == custom.CustomId);
                            if (selections != null)
                            {
                                foreach (var selection in selections.ToList())
                                {
                                    _repository.Delete(selection);
                                }
                            }
                            _repository.Delete(custom);
                        }
                    }
                    _repository.Delete(product);

                    var effectAmount = _repository._context.SaveChanges();
                    RecordChange(request.ShopId, "刪除產品", Extensions.JsonSerialize(request));
                    transaction.Commit();

                    return new APIResult(APIStatus.Success, $"刪除成功，共{effectAmount}筆資料刪除", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "產品刪除失敗");
                    return new APIResult(APIStatus.Fail, "刪除失敗", false);
                }
            }
        }
        public APIResult CreateProduct(ProductInputVM request)
        {
            //create product，且其下的custom、selection都是create；
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var productId = CreateProductOperation(request);
                    foreach(var custom in request.Customs)
                    {
                        CreateCustomOperation(custom, productId);
                    }

                    RecordChange(request.ShopId, "建立產品", Extensions.JsonSerialize(request));
                    transaction.Commit();

                    return new APIResult(APIStatus.Success, "新增成功", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "產品建立失敗");
                    return new APIResult(APIStatus.Fail, "新增失敗", false);
                }
            }

        }
        public APIResult EditProduct(ProductInputVM request)
        {
            //update product，且其下的custom、selection都依照id update、並依delete欄位的資料刪除
            //若custom為new -> create，且其selection都create 
            //若selection為new -> create
            using (var transaction = _repository._context.Database.BeginTransaction())
            {
                try
                {
                    var target = _repository.GetAll<Product>().First(x => x.ProductId == int.Parse(request.Id));
                    target.Name = request.Name.Trim();
                    target.MenuClassId = request.MenuClassId;
                    target.State = TransferProductState(request.State);
                    target.Fig = request.Figure;
                    target.BasicPrice = request.BasicPrice;
                    target.Note = request.ChangeNote;
                    _repository.Update(target);
                    _repository.Save();

                    foreach (var custom in request.Customs)
                    {
                        if (custom.Id.Contains("new"))
                        {
                            CreateCustomOperation(custom, target.ProductId);
                        }
                        else
                        {
                            EditCustomOperation(custom);
                        }
                    }

                    if (request.DeletedCustoms != null)
                    {
                        foreach (var customId in request.DeletedCustoms)
                        {
                            DeleteCustomOperation(int.Parse(customId)); //以下的selection全部刪除
                        }
                    }

                    RecordChange(request.ShopId, "編輯產品", Extensions.JsonSerialize(request));
                    transaction.Commit();

                    return new APIResult(APIStatus.Success, "保存成功", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "產品更新失敗");
                    return new APIResult(APIStatus.Fail, "保存失敗", false);
                }
            }
        }

        //回傳classId
        private int CreateClassOperation(ClassInputVM request)
        {
            var entity = new MenuClass
            {
                ClassName = request.ClassName.Trim(),
                ShopId = request.ShopId,
            };

            var classId = _repository._context.MenuClasses.Add(entity).Entity;
            _repository.Save();

            return classId.MenuClassId;
        }
        //回傳總共刪除的筆數
        private int DeleteClassOperation(ClassInputVM request)
        {
            var menuClass = _repository.GetAll<MenuClass>().First(x => x.MenuClassId == request.MenuClassId);
            var products = _repository.GetAll<Product>().Where(x => x.MenuClassId == menuClass.MenuClassId).ToList();
            var customs = _repository.GetAll<Custom>().Where(x => products.Select(y => y.ProductId).Contains(x.ProductId)).ToList();
            var selections = _repository.GetAll<CustomSelection>().Where(x => customs.Select(y => y.CustomId).Contains(x.CustomId)).ToList();

            foreach (var selection in selections)
            {
                _repository.Delete(selection);
            }

            foreach (var custom in customs)
            {
                _repository.Delete(custom);
            }

            foreach (var product in products)
            {
                _repository.Delete(product);
            }

            _repository.Delete(menuClass);

            var effectAmount = _repository._context.SaveChanges();

            return effectAmount;
        }
        //回傳productId
        private int CreateProductOperation(ProductInputVM request)
        {
            int productState = TransferProductState(request.State);

            var entity = new Product
            {
                Name = request.Name.Trim(),
                MenuClassId = request.MenuClassId,
                State = productState,
                Fig = request.Figure,
                BasicPrice = request.BasicPrice,
                Note = request.ChangeNote
            };
            var addedproduct = _repository._context.Products.Add(entity).Entity;
            _repository.Save();

            return addedproduct.ProductId;
        }

        private object CreateCustomOperation(CustomVM request, int productId)
        {
            var entity = new Custom()
            {
                Name = request.Name.Trim(),
                MaxAmount = request.MaxAmount,
                Necessary = request.Necessary,
                ProductId = productId
            };

            try
            {
                var addedcustom = _repository._context.Customs.Add(entity).Entity;
                _repository.Save();

                foreach(var selection in request.Selections)
                {
                    CreateSelectionOperation(selection, addedcustom.CustomId);
                }
                //CreateSelectionsOperation(request.Selections, addedcustom.CustomId);

                return new APIResult(APIStatus.Success, "題型建立成功", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "題型建立失敗");
                return ex; //讓rollback發生
            }
        }
        private object CreateSelectionOperation(SelectionVM request, int customId)
        {
            var entity = new CustomSelection
            {
                CustomId = customId,
                Name = request.Name.Trim(),
                AddPrice = request.Price
            };

            try
            {
                _repository.Create(entity);
                _repository.Save();

                return new APIResult(APIStatus.Success, "選項建立成功", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "選項建立失敗");
                return ex; //讓rollback發生
            }
        }
        private object CreateSelectionsOperation(IEnumerable<SelectionVM> request, int customId)
        {
            var entities = request.Select(x => new CustomSelection
            {
                CustomId = customId,
                Name = x.Name.Trim(),
                AddPrice = x.Price
            });

            try
            {
                _repository._context.AddRangeAsync(entities);
                
                _repository.Save();

                return new APIResult(APIStatus.Success, "選項建立成功", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "選項建立失敗");
                return ex; //讓rollback發生
            }
        }
        private object EditCustomOperation(CustomVM request)
        {
            try
            {
                var target = _repository.GetAll<Custom>().First(x => x.CustomId == int.Parse(request.Id));
                target.Name = request.Name.Trim();
                target.MaxAmount = request.MaxAmount;
                target.Necessary = request.Necessary;

                _repository.Update(target);
                _repository.Save();

                foreach (var selection in request.Selections)
                {
                    if (selection.Id.Contains("new"))
                    {
                        CreateSelectionOperation(selection, target.CustomId);
                    }
                    else
                    {
                        EditSelectionOperation(selection);
                    }
                }

                if (request.DeletedSelections != null)
                {
                    foreach (var selectionId in request.DeletedSelections)
                    {
                        DeleteSelectionOperation(int.Parse(selectionId));
                    }
                }

                return new APIResult(APIStatus.Success, "題型更新成功", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "題型更新失敗");
                return ex; //讓rollback發生
            }
        }
        private object EditSelectionOperation(SelectionVM request)
        {
            try
            {
                var target = _repository.GetAll<CustomSelection>().First(x => x.CustomSelectionId == int.Parse(request.Id));
                target.Name = request.Name.Trim();
                target.AddPrice = request.Price;

                _repository.Update(target);
                _repository.Save();
                return new APIResult(APIStatus.Success, "選項更新成功", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "選項更新失敗");
                return ex; //讓rollback發生
            }
        }
        private object DeleteCustomOperation(int request)
        {
            try
            {
                var custom = _repository.GetAll<Custom>().First(x => x.CustomId == request);
                var selections = _repository.GetAll<CustomSelection>().Where(x => x.CustomId == custom.CustomId);
                if (selections != null)
                {
                    foreach (var selection in selections.ToList())
                    {
                        _repository.Delete(selection);
                    }
                }
                _repository.Delete(custom);

                var effectAmount = _repository._context.SaveChanges();
                return new APIResult(APIStatus.Success, $"題型刪除成功，共{effectAmount}筆資料刪除", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "題型刪除失敗");
                return ex; //讓rollback發生
            }
        }
        private object DeleteSelectionOperation(int request)
        {
            try
            {
                var entity = _repository.GetAll<CustomSelection>().First(x => x.CustomSelectionId == request);
                _repository.Delete(entity);

                _repository.Save();
                return new APIResult(APIStatus.Success, "選項刪除成功", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "選項刪除失敗");
                return ex; //讓rollback發生
            }
        }
        private int TransferProductState(string state)
        {
            int productState;

            switch (state)
            {
                case "上架":
                    productState = (int)ProductEnum.StateEnum.Enable;
                    break;
                case "下架":
                    productState = (int)ProductEnum.StateEnum.Disable;
                    break;
                case "刪除":
                    productState = (int)ProductEnum.StateEnum.Deleted;
                    break;
                default:
                    productState = (int)ProductEnum.StateEnum.Enable;
                    break;
            }

            return productState;
        }
        private void RecordChange(int shopId, string title, string content)
        {
            try
            {
                var history = new MenuHistory()
                {
                    ShopId = shopId,
                    UpdateTitle = title,
                    UpdateContent = content,
                    UpdateTime = DateTime.UtcNow,
                };
                _repository.Create(history);
                _repository.Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "變更記錄失敗");
            }
        }
    }
}


