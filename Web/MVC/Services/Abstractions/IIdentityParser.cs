namespace MVC.Services.Abstractions
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
