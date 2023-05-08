using WowDin.Frontstage.Common;
using WowDin.Frontstage.Models.Dto.Order;
using WowDin.Frontstage.Models.Dto.OrderDetails;
using WowDin.Frontstage.Models.Dto.Store;

namespace WowDin.Frontstage.Services.Order.Interface
{
    public interface IOrderService
    {
        public GetOrdersDto GetOrderById(int id);
        public GetOrderDetailDto GetOrderDetailDtoById(int id);
        public GetCartsDto GetCartByUserId(int id);
        public GetECPayDataDto GetECPayDataById(int id);
        public GetAllDataDto GetAllData();
        public GetCartCompleteDto GetCartComplete(string orderStamp);
        public GetCartCompleteDto DataUpdate(int cartId, string orderStamp, string paymentDate);
        public OperationResult DeleteUserCart(DeleteCartDto cartId);
        public GetCartDetailsDto GetCartDetails(int userId, int shopId);

        //public GetCartDetailsDto GetCartDetails(int cartId);
        public OperationResult CreateComment(CreateCommentInputDto input);
        public OperationResult CreateOrder(AddCartDetailsInputDto request);
        public OperationResult OrderCancelled(CancelOrderDto request);

        //public bool IsExistComment(int orderid);
        public bool IsExistOrder(int orderid);
        public OperationResult AddProductToCart(AddProductToCartDto addCartDto);
        public OperationResult DeleteProductFromCart(DeleteCartDetailDto cartId);
        public OperationResult AddCartByGroup(AddProductToCartDto addCartDto);
        public GetUserAndShopFromGroupIdDto GetUserAndShopFromGroupId(string groupId);
        public ProductInCartDto GetAmountInCart(int leaderId, int shopId);
    }
}
