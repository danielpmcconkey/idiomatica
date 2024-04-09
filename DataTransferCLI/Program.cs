using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.DAL;

namespace DataTransferCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TransferHelper helper = new TransferHelper();
            helper.TransferData();
        }
    }
}
