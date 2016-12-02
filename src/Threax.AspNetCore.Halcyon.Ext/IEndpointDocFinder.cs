namespace Threax.AspNetCore.Halcyon.Ext
{
    public interface IEndpointDocFinder
    {
        EndpointDescription FindDoc(string groupName, string method, string relativePath);
    }
}