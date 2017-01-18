using System;

namespace OmniChannel.VideoChatConnector.APIs.Rentals
{
	using System.Web.Mvc;
	using App_Start;
	using MongoDB.Bson;
	using MongoDB.Driver;
	using MongoDB.Driver.Builders;

	/// <summary>
	/// 
	/// </summary>
	public class RentalsController : Controller
	{
		//public readonly RealEstateContext Context = new RealEstateContext();

		//public ActionResult Index()
		//{
		//	var rentals = Context.Rentals.FindAll();
		//	return View(rentals);
		//}
        public readonly VideoConnectorContext Context = new VideoConnectorContext();
		public ActionResult Post()
		{
			return View();
		}

	    public ActionResult Index(RentalsFilter filters)
	    {
	        var rentals = FilterRentals(filters);
            var model = new RentalsList()
            {
                Rentals = rentals,
                Filters = filters
            };
	        return View(model);
	    }

	    private MongoCursor<Rental> FilterRentals(RentalsFilter filters)
	    {
	      
            if (!filters.PriceFilter.HasValue)
	        {
                 return Context.Rentals.FindAll();
            }
            var query = Query<Rental>.LTE(r=>r.Price,filters.PriceFilter);
	        return Context.Rentals.Find(query);
	    }

	    [HttpPost]
	    public ActionResult Post(PostRental postRental)
	    {
            var rental= new Rental(postRental);
	        Context.Rentals.Insert(rental);
            return RedirectToAction("Index");
	    }

        //	var rental = new Rental(postRental);
        //	Context.Rentals.Insert(rental);
        //	return RedirectToAction("Index");
        //}

        public ActionResult AdjustPrice(string id)
        {
            var rental = GetRental(id);
            return View(rental);
        }

        private Rental GetRental(string id)
        {
            var rental = Context.Rentals.FindOneById(new ObjectId(id));
            return rental;
        }

        [HttpPost]
        public ActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            var rental = GetRental(id);
            rental.AdjustPrice(adjustPrice);
            Context.Rentals.Save(rental);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AdjustPriceUsingModification(string id, AdjustPrice adjustPrice)
        {
            var rental = GetRental(id);
            var adjustment = new PriceAdjustment(adjustPrice, rental.Price);
            var modificationUpdate = Update<Rental>
                .Push(r => r.Adjustments, adjustment)
                .Set(r => r.Price, adjustPrice.NewPrice);
            Context.Rentals.Update(Query.EQ("_id", new ObjectId(id)), modificationUpdate);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            Context.Rentals.Remove(Query.EQ("_id", new ObjectId(id)));
            return RedirectToAction("Index");
        }
    }
}