using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Common.Queries.GetHomepageimages
{
    public interface IGetHomepageImagesService
    {
        ResultDto<List<HomepageImagesDto>> Execute();
    }

    public class GetHomepageImagesService : IGetHomepageImagesService
    {
        private readonly IDataBaseContext _context;
        public GetHomepageImagesService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<HomepageImagesDto>> Execute()
        {
            var images = _context.HomePageImages.OrderByDescending(p => p.Id).Select(p => new HomepageImagesDto()
            {
                Id = p.Id,
                ImageLocation = p.ImageLocation,
                Link = p.Link,
                Src = p.Src,
            }).ToList();

            return new ResultDto<List<HomepageImagesDto>>()
            {
                Data = images,
                IsSuccess = true
            };
        }
    }


    public class HomepageImagesDto
    {
        public long Id { get; set; }
        public string Src { get; set; }
        public string Link { get; set; }
        public ImageLocation ImageLocation { get; set; }
    }
}
