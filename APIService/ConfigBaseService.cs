using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;


namespace DotnetCoreWCF
{
    /// <summary>
    /// Cấu hình cơ bản cho service
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// trungtq 25/6/2013   created
    /// </Modified>
    public class ConfigBaseService<TClient> where TClient : ICommunicationObject, new()
    {

        private static object _syncRoot = new object();

        private static TClient _client;

        /// <summary>
        /// Đối tượng client cần làm việc
        /// </summary>
        protected TClient Client
        {
            get
            {
                try
                {
                    if (_client == null)
                    {
                        lock (_syncRoot)
                        {
                            _client = new TClient();
                            _client.Open();
                        }
                    }
                    else
                    {
                        if (_client.State == CommunicationState.Closed)
                        {
                            lock (_syncRoot)
                            {
                                _client = new TClient();
                                _client.Open();
                            }
                        }
                        else if (_client.State == CommunicationState.Faulted)
                        {
                            _client.Close();
                            _client = new TClient();
                            _client.Open();
                        }
                        else if (_client.State == System.ServiceModel.CommunicationState.Created)
                        {
                            _client.Close();
                            _client = new TClient();
                            _client.Open();
                        }
                    }
                }

                catch (CommunicationException ex1)
                {
                    lock (_syncRoot)
                    {
                        _client?.Abort();

                        _client = new TClient();
                        _client.Open();
                    }
                }
                catch (TimeoutException ex2)
                {
                    lock (_syncRoot)
                    {
                        _client?.Abort();
                        _client = new TClient();
                        _client.Open();
                    }
                }
                catch (Exception ex3)
                {
                    lock (_syncRoot)
                    {
                        _client?.Abort();
                        _client = new TClient();
                        _client.Open();
                    }
                }
                return _client;
            }
        }
    
    }
}
