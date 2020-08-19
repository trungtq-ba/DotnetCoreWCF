using GpsMemService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetCoreWCF
{
    /// <summary>
    /// Service lam viec voi tang duoi voi minhnx
    /// 1. Dung cho gan cong ty tu thang cha sang thang con.
    /// 2. Memory Cache
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// trungtq  23/8/2012   created
    /// </Modified>
    public class GPSMemoryService : ConfigBaseService<SyncSystemServiceClient>
    {

        //private string AccessTokenGps { get; set; }

        //private string ClientTag { get; set; }

        private string AccessTokenGps => null;

        private string ClientTag => null;

        /// <summary>
        /// Message thong bao loi
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Chuan bi cac du lieu dau vao cho request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  23/8/2012   created
        /// </Modified>
        private T PrepareRequestGps<T>(T request) where T : RequestBase
        {
            request.RequestId = Guid.NewGuid().ToString();  // Generates unique request id
            request.ClientTag = ClientTag;
            request.AccessToken = AccessTokenGps;

            return request;
        }

        /// <summary>
        /// Khoi tao danh sach xe Online tu Memmory lan dau
        /// </summary>
        /// <param name="xnCode">Ma XN</param>
        /// <param name="companyId">Cong ty ID</param>
        /// <param name="groupIDs">Danh sach nhom xe</param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  30/11/2012   created
        /// </Modified>
        public List<VehicleExtend> InitVehicleOnline(int xnCode, int companyId, string groupIDs, string accessToken)
        {
            List<VehicleExtend> listVehicleOnline = null;
            try
            {
               
                VehilceOnlineRequest rq = PrepareRequestGps(new VehilceOnlineRequest());
                rq.XNCode = xnCode;
                rq.CompanyID = companyId;
                rq.AccessToken = accessToken;

                //Lay ra mang nhom xe theo chuoi truyen vao dang '1,3,5,6,7'.
                var arrGroupId = groupIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(g => Convert.ToInt32(g)).ToArray();

                rq.AryGroupId = (arrGroupId.Length > 0) ? arrGroupId : null;
                VehicleOnlineResponse rp =  Client.InitOnlineVerhicleAsync(rq).Result;
                listVehicleOnline = rp?.Onlines?.ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return listVehicleOnline;

        }
    }
}
