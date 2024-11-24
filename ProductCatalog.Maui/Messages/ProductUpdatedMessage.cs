using CommunityToolkit.Mvvm.Messaging.Messages;
using Resources.Shared.Models;

namespace ProductCatalog.Maui.Messages;

public class ProductUpdatedMessage : ValueChangedMessage<Product>
{
    public ProductUpdatedMessage(Product product) : base(product)
    {
    }
}