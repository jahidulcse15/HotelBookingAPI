namespace HotelBookingAPI_Project.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int RoomNumber {  get; set; }
        public decimal PricePerNight {  get; set; }
        public string RoomType {  get; set; }
        public bool IsAvaiable {  get; set; }
        public int HotelId {  get; set; }
        public Hotel? Hotel { get; set; }
    }
}
