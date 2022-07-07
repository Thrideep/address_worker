using Api.Abstractions.Extensions;
using System;
using System.Net;

namespace Api.Abstractions.Validation
{
    public static class AddressValidator
    {
        public static bool IsAddressValid(string address)
        {
            return GetAddressType(address) != AddressType.None;
        }

        public static AddressType GetAddressType(string address)
        {
            if (address.IsNullOrWhiteSpace())
            {
                return AddressType.None;
            }

            AddressType addressType;
            //URI validaiton
            try
            {
                _ = new Uri(address);
                addressType = AddressType.Domain;
            }
            catch (Exception)
            {
                addressType = AddressType.None;
            }

            if (addressType == AddressType.None)
            {
                try
                {
                    Uri uri = new($"http://{address}");

                    if (uri.HostNameType is UriHostNameType.IPv4 or UriHostNameType.IPv6)
                    {
                        if (uri.ToString().Equals(address, StringComparison.OrdinalIgnoreCase) is false)
                        {
                            addressType = AddressType.None;
                        }
                    }
                    else
                    {
                        addressType = AddressType.Domain;
                    }
                }
                catch (Exception)
                {
                    addressType = AddressType.None;
                }
            }

            if (addressType == AddressType.None)
            {
                //IP v4/v6 validation
                try
                {
                    IPAddress ipAddress = IPAddress.Parse(address);
                    addressType = ipAddress.ToString().Equals(address, StringComparison.OrdinalIgnoreCase) ? AddressType.IPAddress : AddressType.None;
                }
                catch (Exception)
                {
                    addressType = AddressType.None;
                }
            }

            return addressType;
        }

    }
}
