namespace Api.Devion.Helpers.TimeChimp
{
    public class VehicleHelper : TimeChimpHelper
    {
        public VehicleHelper(WebClient clientTC) : base(clientTC)
        {
        }

        public VehicleTimeChimp GetVehicle(int vehicleId)
        {
            //get data from timechimp
            string response = TCClient.GetAsync($"mileageVehicles?$filter=id eq {vehicleId}");

            //convert data to vehicleTimeChimp object
            VehicleTimeChimp vehicle = JsonTool.ConvertTo<ResponseTCVehicle>(response).Result[0];

            return vehicle;
        }
    }
}
