using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantReview.Web.Controllers
{
    /// <summary>
    /// A base class for an API controller without view support.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Mapper for DTO to Entity (and reverse) conversion.
        /// </summary>
        protected IMapper Mapper { get; }

        /// <inheritdoc />
        public ApiControllerBase(IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

    }
}
