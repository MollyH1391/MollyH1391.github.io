using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WowDin.Backstage.Models.Entity
{
    public partial class WowdinDbContext : DbContext
    {
        public WowdinDbContext()
        {
        }

        public WowdinDbContext(DbContextOptions<WowdinDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressOption> AddressOptions { get; set; }
        public virtual DbSet<Advertise> Advertises { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<BrandFigure> BrandFigures { get; set; }
        public virtual DbSet<BrandHistory> BrandHistories { get; set; }
        public virtual DbSet<CardType> CardTypes { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<Catagory> Catagories { get; set; }
        public virtual DbSet<CatagorySet> CatagorySets { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<CouponContainer> CouponContainers { get; set; }
        public virtual DbSet<CouponProduct> CouponProducts { get; set; }
        public virtual DbSet<Custom> Customs { get; set; }
        public virtual DbSet<CustomSelection> CustomSelections { get; set; }
        public virtual DbSet<CustomSet> CustomSets { get; set; }
        public virtual DbSet<CustomToSet> CustomToSets { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<MenuClass> MenuClasses { get; set; }
        public virtual DbSet<MenuHistory> MenuHistories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<PointHistory> PointHistories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<ShopFigure> ShopFigures { get; set; }
        public virtual DbSet<ShopHistory> ShopHistories { get; set; }
        public virtual DbSet<ShopMethod> ShopMethods { get; set; }
        public virtual DbSet<ShopPaymentType> ShopPaymentTypes { get; set; }
        public virtual DbSet<Takeout> Takeouts { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<UserCard> UserCards { get; set; }
        public virtual DbSet<Verification> Verifications { get; set; }
        public virtual DbSet<Website> Websites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=tcp:bs-2021-winter-wowdin.database.windows.net,1433;Initial Catalog=WowdinDb;Persist Security Info=False;User ID=bs;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AddressOption>(entity =>
            {
                entity.HasIndex(e => e.UserAccountId, "IX_AddressOptions_UserAccountId");

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.City)
                    .HasMaxLength(10)
                    .HasComment("Enum");

                entity.Property(e => e.District)
                    .HasMaxLength(10)
                    .HasComment("Enum");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("自訂名稱");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.AddressOptions)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AddressOptions_UserAccount");
            });

            modelBuilder.Entity<Advertise>(entity =>
            {
                entity.ToTable("Advertise");

                entity.HasIndex(e => e.CouponId, "IX_Advertise_CouponId");

                entity.Property(e => e.CouponId).HasComment("");

                entity.Property(e => e.Img)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.Advertises)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("FK_Advertise_Promotion");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Booking");

                entity.HasIndex(e => e.ShopMethodId, "IX_Booking_ShopMethodId");

                entity.Property(e => e.待討論)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.ShopMethod)
                    .WithMany()
                    .HasForeignKey(d => d.ShopMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_ShopMethod");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.HasIndex(e => e.VerificationId, "IX_Brand_VerificationId");

                entity.Property(e => e.CardImgUrl).IsRequired();

                entity.Property(e => e.FirstColor)
                    .HasMaxLength(10)
                    .IsFixedLength(true)
                    .HasComment("null-blue");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SecondColor)
                    .HasMaxLength(10)
                    .IsFixedLength(true)
                    .HasComment("null-black");

                entity.Property(e => e.Slogen).HasMaxLength(50);

                entity.Property(e => e.VerificationId).HasComment("");

                entity.Property(e => e.Verified).HasComment("0是未驗證；1是已驗證");

                entity.HasOne(d => d.Verification)
                    .WithMany(p => p.Brands)
                    .HasForeignKey(d => d.VerificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Brand_Verification");
            });

            modelBuilder.Entity<BrandFigure>(entity =>
            {
                entity.ToTable("BrandFigure");

                entity.HasIndex(e => e.BrandId, "IX_BrandFigure_BrandId");

                entity.Property(e => e.AltText)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url).HasComment("預設首頁");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.BrandFigures)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BrandFigure_Brand");
            });

            modelBuilder.Entity<BrandHistory>(entity =>
            {
                entity.ToTable("BrandHistory");

                entity.HasIndex(e => e.BrandId, "IX_BrandHistory_BrandId");

                entity.Property(e => e.UpdateContent)
                    .IsRequired()
                    .HasComment("變更的內容");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasComment("資料更新時間");

                entity.Property(e => e.UpdateTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("變更的資料名稱");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.BrandHistories)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BrandDetail_Brand");
            });

            modelBuilder.Entity<CardType>(entity =>
            {
                entity.ToTable("CardType");

                entity.HasIndex(e => e.BrandId, "IX_CardType_BrandId");

                entity.Property(e => e.CardImgUrl).IsRequired();

                entity.Property(e => e.CardLevel).HasComment("至少為1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("例如迷點、迷金");

                entity.Property(e => e.Range).HasComment("該level的點數範圍，null就無上限");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.CardTypes)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CardType_Brand");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.HasIndex(e => e.ShopId, "IX_Cart_ShopId");

                entity.HasIndex(e => e.UserAccountId, "IX_Cart_UserAccountId");

                entity.Property(e => e.GroupCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Orderdate).HasColumnType("datetime");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cart_Shop");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cart_UserAccount");
            });

            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.ToTable("CartDetail");

                entity.HasIndex(e => e.CartId, "IX_CartDetail_CartId");

                entity.HasIndex(e => e.ProductId, "IX_CartDetail_ProductId");

                entity.Property(e => e.Note).IsRequired();

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartDetails)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CartDetail_Cart");
            });

            modelBuilder.Entity<Catagory>(entity =>
            {
                entity.ToTable("Catagory");

                entity.Property(e => e.Fig)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CatagorySet>(entity =>
            {
                entity.ToTable("CatagorySet");

                entity.HasIndex(e => e.BrandId, "IX_CatagorySet_BrandId");

                entity.HasIndex(e => e.CatagoryId, "IX_CatagorySet_CatagoryId");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.CatagorySets)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatagorySet_Brand");

                entity.HasOne(d => d.Catagory)
                    .WithMany(p => p.CatagorySets)
                    .HasForeignKey(d => d.CatagoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatagorySet_Catagory");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.HasIndex(e => e.ShopId, "IX_Comment_ShopId");

                entity.Property(e => e.Comment1)
                    .HasColumnName("Comment")
                    .HasComment("購買後的評價");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasComment("為了店家查詢訂單評價");

                entity.Property(e => e.ShopId).HasComment("為了計算店家星級");

                entity.Property(e => e.Star).HasComment("購買後的評分");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Shop");
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.ToTable("Coupon");

                entity.HasIndex(e => e.ShopId, "IX_Coupon_ShopId");

                entity.Property(e => e.CouponId).HasComment("");

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Description).HasComment("活動描述");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.DiscountType).HasComment("打折或扣款或送、Enum");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.MaxAmount).HasComment("null無上限");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.ThresholdAmount)
                    .HasColumnType("decimal(18, 0)")
                    .HasComment("數量(預設1，加價購的數量門檻為2)、金額");

                entity.Property(e => e.ThresholdType).HasComment("滿量(預設)、滿額");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Coupons)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Promotion_Shop");
            });

            modelBuilder.Entity<CouponContainer>(entity =>
            {
                entity.ToTable("CouponContainer");

                entity.HasIndex(e => e.CouponId, "IX_CouponContainer_CouponId");

                entity.HasIndex(e => e.UserAccountId, "IX_CouponContainer_UserAccountId");

                entity.Property(e => e.CouponState).HasComment("Enum");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.CouponContainers)
                    .HasForeignKey(d => d.CouponId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouponContainer_Promotion");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.CouponContainers)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouponContainer_UserAccount");
            });

            modelBuilder.Entity<CouponProduct>(entity =>
            {
                entity.ToTable("CouponProduct");

                entity.HasIndex(e => e.ProductId, "IX_CouponProduct_ProductId");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.CouponProducts)
                    .HasForeignKey(d => d.CouponId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionProduct_Promotion");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CouponProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionProduct_Product");
            });

            modelBuilder.Entity<Custom>(entity =>
            {
                entity.ToTable("Custom");

                entity.HasIndex(e => e.ProductId, "IX_Custom_ProductId");

                entity.Property(e => e.MaxAmount).HasComment("非必選的多選項目的數量上限，null為無上限，若為必選則為1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("客製化項目的標題，例如冰量");

                entity.Property(e => e.Necessary).HasComment("必選或非必選");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Customs)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Custom_Product");
            });

            modelBuilder.Entity<CustomSelection>(entity =>
            {
                entity.ToTable("CustomSelection");

                entity.HasIndex(e => e.CustomId, "IX_CustomSelection_CustomId");

                entity.Property(e => e.AddPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CustomId).HasComment("屬於什麼客製化項目，例如冰量");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("例如少冰");

                entity.HasOne(d => d.Custom)
                    .WithMany(p => p.CustomSelections)
                    .HasForeignKey(d => d.CustomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppendSelection_Appendage");
            });

            modelBuilder.Entity<CustomSet>(entity =>
            {
                entity.ToTable("CustomSet");

                entity.HasIndex(e => e.ShopId, "IX_CustomSet_ShopId");

                entity.Property(e => e.SetName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("自訂組合名稱，前端設定預設值");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.CustomSets)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomSet_Shop");
            });

            modelBuilder.Entity<CustomToSet>(entity =>
            {
                entity.ToTable("CustomToSet");

                entity.HasIndex(e => e.CustomSetId, "IX_CustomToSet_CustomSetId");

                entity.HasOne(d => d.CustomSet)
                    .WithMany(p => p.CustomToSets)
                    .HasForeignKey(d => d.CustomSetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomToSet_CustomSet");
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.ToTable("Delivery");

                entity.HasIndex(e => e.ShopMethodId, "IX_Delivery_ShopMethodId");

                entity.Property(e => e.DeliveryFee)
                    .HasColumnType("decimal(18, 0)")
                    .HasComment("可為0");

                entity.Property(e => e.HigherDistance).HasComment("不可能距離無限吧..?");

                entity.Property(e => e.LowerDistance).HasComment("");

                entity.Property(e => e.PriceThreshold)
                    .HasColumnType("decimal(18, 0)")
                    .HasComment("低消");

                entity.HasOne(d => d.ShopMethod)
                    .WithMany(p => p.Deliveries)
                    .HasForeignKey(d => d.ShopMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivery_ShopMethod");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => e.FavoraiteId)
                    .HasName("PK_Favorite_1");

                entity.ToTable("Favorite");

                entity.HasIndex(e => e.ShopId, "IX_Favorite_ShopId");

                entity.HasIndex(e => e.UserAccountId, "IX_Favorite_UserAccountId");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Favorite_Shop");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Favorite_UserAccount");
            });

            modelBuilder.Entity<MenuClass>(entity =>
            {
                entity.ToTable("MenuClass");

                entity.HasIndex(e => e.ShopId, "IX_MenuClass_ShopId");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("menu中的分類");

                entity.Property(e => e.ShopId).HasComment("屬於哪個Shop");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.MenuClasses)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuClass_Shop");
            });

            modelBuilder.Entity<MenuHistory>(entity =>
            {
                entity.ToTable("MenuHistory");

                entity.HasIndex(e => e.ShopId, "IX_ProductHistory_ProductId");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateTitle).HasMaxLength(50);

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.MenuHistories)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenuHistory_Shop1");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasIndex(e => e.ShopId, "IX_Order_ShopId");

                entity.HasIndex(e => e.UserAcountId, "IX_Order_UserAcountID");

                entity.Property(e => e.City).HasComment("Enum");

                entity.Property(e => e.CouponId).HasComment("");

                entity.Property(e => e.DeliveryFee).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.District).HasComment("Enum");

                entity.Property(e => e.Message).HasComment("下訂時給店家的留言");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderState).HasComment("Enum、送出訂單(未接單)、已接單、進行中(團員送出訂單後)、完成、失敗");

                entity.Property(e => e.PayDate).HasColumnType("datetime");

                entity.Property(e => e.PayState).HasComment("Enum 退款");

                entity.Property(e => e.PaymentType).HasComment("Enum 找團長付款");

                entity.Property(e => e.ReceiptType).HasComment("Enum");

                entity.Property(e => e.TakeMethodId).HasComment("取餐方式");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasComment("初始是OrderDate");

                entity.Property(e => e.UsePoint).HasComment("會員卡使用點數");

                entity.Property(e => e.UserAcountId)
                    .HasColumnName("UserAcountID")
                    .HasComment("團長/付款人");

                entity.Property(e => e.Vatnumber)
                    .HasMaxLength(50)
                    .HasColumnName("VATnumber");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Shop");

                entity.HasOne(d => d.UserAcount)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserAcountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_UserAccount");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.HasIndex(e => e.OrderId, "IX_OrderDetail_OrderId");

                entity.HasIndex(e => e.UserAccountId, "IX_OrderDetail_UserAccountId");

                entity.Property(e => e.Discount)
                    .HasColumnType("decimal(18, 0)")
                    .HasComment("實際扣款數");

                entity.Property(e => e.IsPaid).HasComment("null是團長");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasComment("規格的整合資料");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UserAccountId).HasComment("團長或團員");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_UserAccount");
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("Platform");

                entity.Property(e => e.PlatformId).HasComment("網路平台");

                entity.Property(e => e.Logo)
                    .HasMaxLength(50)
                    .HasComment("平台小圖。若為官網則為null");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true)
                    .HasComment("例如官網、facebook。");
            });

            modelBuilder.Entity<PointHistory>(entity =>
            {
                entity.ToTable("PointHistory");

                entity.HasIndex(e => e.OrderId, "IX_PointHistory_OrderId");

                entity.HasIndex(e => e.UserAccountId, "IX_PointHistory_UserAccountId");

                entity.HasIndex(e => e.UserCardId, "IX_PointHistory_UserCardId");

                entity.Property(e => e.ConsumeType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true)
                    .HasComment("有可能現場消費");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PointHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_PointHistory_Order");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.PointHistories)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointHistory_UserAccount");

                entity.HasOne(d => d.UserCard)
                    .WithMany(p => p.PointHistories)
                    .HasForeignKey(d => d.UserCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PointHistory_UserCard");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.MenuClassId, "IX_Product_MenuClassId");

                entity.Property(e => e.BasicPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Fig)
                    .HasMaxLength(50)
                    .HasColumnName("FIG");

                entity.Property(e => e.MenuClassId).HasComment("同個商品不會在不同的分類");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note)
                    .HasColumnType("ntext")
                    .HasComment("用來儲存變更紀錄(json檔)");

                entity.Property(e => e.State).HasComment("上架、下架、已刪除<Enum>");

                entity.HasOne(d => d.MenuClass)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.MenuClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_MenuClass");
            });

            modelBuilder.Entity<Response>(entity =>
            {
                entity.ToTable("Response");

                entity.HasIndex(e => e.BrandId, "IX_Response_BrandId");

                entity.HasIndex(e => e.ShopId, "IX_Response_ShopId");

                entity.HasIndex(e => e.UserAccountId, "IX_Response_UserAccountId");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ResponseContent).IsRequired();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Response_Brand");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("FK_Response_Shop");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Response_UserAccount");
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.ToTable("Shop");

                entity.HasIndex(e => e.BrandId, "IX_Shop_BrandId");

                entity.HasIndex(e => e.VerificationId, "IX_Shop_VerificationId");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("區之後的地址");

                entity.Property(e => e.City).HasComment("Enum");

                entity.Property(e => e.District).HasComment("Enum");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OpenDayList)
                    .IsRequired()
                    .HasComment("營業日List<Enum>");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.PreOrder).HasComment("非營業時是否提供預訂餐");

                entity.Property(e => e.PriceLimit)
                    .HasColumnType("decimal(18, 0)")
                    .HasComment("現金單筆消費上限，null為無上限");

                entity.Property(e => e.State).HasComment("Enum，營業中、休息、歇業");

                entity.Property(e => e.Verified).HasComment("0是未驗證；1是已驗證");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shop_Brand");

                entity.HasOne(d => d.Verification)
                    .WithMany(p => p.Shops)
                    .HasForeignKey(d => d.VerificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shop_Verification");
            });

            modelBuilder.Entity<ShopFigure>(entity =>
            {
                entity.ToTable("ShopFigure");

                entity.HasIndex(e => e.BrandId, "IX_ShopFigure_BrandId");

                entity.Property(e => e.AltText)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Path).IsRequired();

                entity.Property(e => e.Sort).HasComment("排序");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.ShopFigures)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShopFigure_Brand");
            });

            modelBuilder.Entity<ShopHistory>(entity =>
            {
                entity.ToTable("ShopHistory");

                entity.HasIndex(e => e.ShopId, "IX_ShopHistory_ShopId");

                entity.Property(e => e.UpdateContent).IsRequired();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateTitle)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Updator).HasComment("Enum，由誰更新，本店或品牌");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopHistories)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShopDetail_Shop");
            });

            modelBuilder.Entity<ShopMethod>(entity =>
            {
                entity.ToTable("ShopMethod");

                entity.HasIndex(e => e.ShopId, "IX_ShopMethod_ShopId");

                entity.Property(e => e.ShopId).HasComment("");

                entity.Property(e => e.TakeMethod).HasComment("Enum 自取、外送、訂位");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopMethods)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShopMethod_Shop");
            });

            modelBuilder.Entity<ShopPaymentType>(entity =>
            {
                entity.HasIndex(e => e.ShopId, "IX_ShopPaymentTypes_ShopId");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopPaymentTypes)
                    .HasForeignKey(d => d.ShopId);
            });

            modelBuilder.Entity<Takeout>(entity =>
            {
                entity.ToTable("Takeout");

                entity.HasIndex(e => e.ShopMethodId, "IX_Takeout_ShopMethodId");

                entity.Property(e => e.Condition)
                    .HasMaxLength(10)
                    .IsFixedLength(true)
                    .HasComment("等待時間可能因該店家的品項種類有差異，考慮改成商品類型");

                entity.HasOne(d => d.ShopMethod)
                    .WithMany(p => p.Takeouts)
                    .HasForeignKey(d => d.ShopMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Takeout_ShopMethod");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccount");

                entity.HasIndex(e => e.VerificationId, "IX_UserAccount_VerificationId");

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.City)
                    .HasColumnName("CIty")
                    .HasComment("Enum");

                entity.Property(e => e.Distrinct).HasComment("Enum");

                entity.Property(e => e.LoginType).HasComment("Enum");

                entity.Property(e => e.NickName)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.RealName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Sex).HasComment("Enum");

                entity.Property(e => e.VerificationId).HasComment("");

                entity.Property(e => e.Verified).HasComment("0是未驗證；1是已驗證");

                entity.HasOne(d => d.Verification)
                    .WithMany(p => p.UserAccounts)
                    .HasForeignKey(d => d.VerificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAccount_Verification");
            });

            modelBuilder.Entity<UserCard>(entity =>
            {
                entity.ToTable("UserCard");

                entity.HasIndex(e => e.BrandId, "IX_UserCard_BrandId");

                entity.HasIndex(e => e.UserAccountId, "IX_UserCard_UserAccountId");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.UserCards)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCard_Brand");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.UserCards)
                    .HasForeignKey(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCard_UserAccount");
            });

            modelBuilder.Entity<Verification>(entity =>
            {
                entity.ToTable("Verification");

                entity.Property(e => e.AccountType).HasComment("Enum；消費者、品牌、店家");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<Website>(entity =>
            {
                entity.ToTable("Website");

                entity.HasIndex(e => e.BrandId, "IX_Website_BrandId");

                entity.HasIndex(e => e.PlatformId, "IX_Website_PlatformId");

                entity.Property(e => e.Path).IsRequired();

                entity.Property(e => e.PlatformId).HasComment("若平台為官網就用品牌Logo。");

                entity.Property(e => e.Webpic).HasComment("各品牌的官網圖");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Websites)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Website_Brand");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Websites)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Website_Platform");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
