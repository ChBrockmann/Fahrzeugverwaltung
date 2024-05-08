using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Requests;

public class GetReservationsInMonthYearRequest
{
    public int Year { get; set; }
    
    public int Month { get; set; }
}