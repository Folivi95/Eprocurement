using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace EGPS.Application.Helpers
{
  public  class PageUtility<T> 
    {
  public    static   Pagination CreateResourcePageUrl(ResourceParameters parameters, string name, PagedList<T> pageData, IUrlHelper helper)
        {
           
            var prevLink = pageData.HasPrevious
                ? CreateResourceUri(parameters, name, ResourceUriType.PreviousPage, helper  )
                : null;
            var nextLink = pageData.HasNext
                ? CreateResourceUri(parameters, name, ResourceUriType.NextPage, helper)
                : null;
            var currentLink = CreateResourceUri(parameters, name, ResourceUriType.CurrentPage, helper);

           
            var pagination = new Pagination
            {
                currentPage = currentLink,
                nextPage = nextLink,
                previousPage = prevLink,
                totalPages = pageData.TotalPages,
                perPage = pageData.PageSize,
                totalEntries = pageData.TotalCount
            };

            return pagination;
        } 

  public    static   Pagination CreateTransactionPageUrl(TransactionParameters parameters, string name, PagedList<T> pageData, IUrlHelper helper)
  {
           
      var prevLink = pageData.HasPrevious
          ? CreateResourceUri(parameters, name, ResourceUriType.PreviousPage, helper  )
          : null;
      var nextLink = pageData.HasNext
          ? CreateResourceUri(parameters, name, ResourceUriType.NextPage, helper)
          : null;
      var currentLink = CreateResourceUri(parameters, name, ResourceUriType.CurrentPage, helper);

           
      var pagination = new Pagination
      {
          currentPage = currentLink,
          nextPage = nextLink,
          previousPage = prevLink,
          totalPages = pageData.TotalPages,
          perPage = pageData.PageSize,
          totalEntries = pageData.TotalCount
      };

      return pagination;
  } 

        private static string CreateResourceUri(ResourceParameters parameters, string name, ResourceUriType type, IUrlHelper url)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return url.Link(name,
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            parameters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return url.Link(name,
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            parameters.PageSize
                        });

                default:
                    return url.Link(name,
                        new
                        {
                            parameters.PageNumber,
                            parameters.PageSize
                        });
            }

        }
       

    }
}
