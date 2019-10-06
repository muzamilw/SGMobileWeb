using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.Proxy;
using SG2.CORE.MODAL.ViewModals.Backend.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class ProxyManager
    {
        private readonly ProxyRepository _proxyRepository;
        private readonly SessionManager _sessionManager;

        public ProxyManager()
        {
            _proxyRepository = new ProxyRepository();
            _sessionManager = new SessionManager();
        }

        public ProxyDTO AddProxy(ProxyDTO model)
        {
            try
            {
                model.ProxyId = _proxyRepository.AddProxyIP(model).ProxyId;
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool IsProxyIPNumberExists(string ProxyIPNumber, int ProxyId = 0)
        {
            try
            {
                return _proxyRepository.IsProxyIPNumberExists(ProxyIPNumber, ProxyId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string SaveBadProxyIP(int ProxyId, int ProfileId)
        {
            try
            {
                return _proxyRepository.SaveBadProxyIP(ProxyId, ProfileId);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ProxyDTO UpdateProxy(ProxyDTO model)
        {
            try
            {
                var Proxy = _proxyRepository.UpdateProxyIP(model);
                return Proxy;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<BadProxyViewModel> GetBadProxyIPs(string PageSize, int PageNumber)
        {
            var Model = _proxyRepository.GetBadProxyIPs(PageSize, PageNumber);

            return Model;

        }

        public ProxyDTO GetProxyByID(int ProxyId)
        {
            try
            {
                var cust = _proxyRepository.GetProxyById(ProxyId);
                return cust;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsProxyAssignedIPExists(int ProxyId)
        {
            try
            {
                return _proxyRepository.IsProxyAssignedIPExists(ProxyId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public IList<ProxyListingViewModal> GetProxyData(string SearchCriteria, string PageSize, int PageNumber, int? statusId, int? SupplierId)
        {
            var Model = _proxyRepository.GetProxyData(SearchCriteria, PageNumber, PageSize, statusId, SupplierId);

            return Model;
        }

        public bool DeleteProxyIP(int ProxyId)
        {
            try
            {
                var ProxyIp = _proxyRepository.DeleteProxyIP(ProxyId);
                return ProxyIp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
    }
}
