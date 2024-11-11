namespace sports_up_backend.Constants
{
    /// <summary>
    /// Static class containing the coordinates for predefined locations.
    /// </summary>
    public static class PredefinedLocationCoordinates
    {
        /// <summary>
        /// Dictionary mapping predefined locations to their latitude and longitude.
        /// </summary>
        public static readonly Dictionary<string, (decimal Latitude, decimal Longitude)> Coordinates = new()
            {
                { "Gheorgheni Park",  ((decimal)46.76888, (decimal)23.63307) }, // Coordinates for Gheorgheni Park
                { "Iuliu Hatieganu Park",((decimal) 46.76487,(decimal) 23.55855) },   // Coordinates for Iuliu Hatieganu Park
                { "La Terenuri Park",((decimal) 46.74964,(decimal) 23.55479) }        // Coordinates for La Terenuri Park
            };
    }
}