using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ATVEntity;
using BOATV;
using ATVCommon;

namespace API_IOS.Controllers
{
    public class DetailController : ApiController
    {



        public IHttpActionResult GetDetail(long newsID)
        {
            NewsPublishEntity newsPublishEntity = NewsPublished.NP_TinChiTiet(newsID, true);
            if (newsPublishEntity == null)
            {
                return NotFound();
            }
            return Ok(newsPublishEntity);

        }

    }
}
