using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.JVBox;
using SG2.CORE.MODAL.ViewModals.Backend.JVBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class JVBoxManager
    {
        private readonly JVBoxRepository _jVBoxRepository;
        private readonly SessionManager _sessionManager;
 
        public JVBoxManager()
        {
            _jVBoxRepository = new JVBoxRepository();
            _sessionManager = new SessionManager();
        }


        public JVBoxDTO AddJVBox(JVBoxDTO entity)
        {
            try
            {
                      _jVBoxRepository.AddJVBox(entity);
                    return entity;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JVBoxDTO UpdateJVBox(JVBoxDTO entity)
        {
            try
            {
                _jVBoxRepository.UpdateJVBox(entity);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JVBoxDTO GetJVBoxById(int JVBoxId)
        {
            try
            {
                JVBoxDTO jVBoxDTO = new JVBoxDTO();
                jVBoxDTO =_jVBoxRepository.GetJVBoxById(JVBoxId);
                return jVBoxDTO;
            }            
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsJVBoxExists(string JVBoxName, int id = 0)
        {
            try
            {
                return _jVBoxRepository.IsJVBoxExists(JVBoxName, id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool JVBoxSetServerRunningStatus(int jvboxid, int serverrunningstatusId)
        {
            try
            {
                return _jVBoxRepository.JVBoxSetServerRunningStatus(jvboxid, serverrunningstatusId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool JVBoxGetServerRunningStatus(int jvboxid)
        {
            try
            {
                return _jVBoxRepository.JVBoxGetServerRunningStatus(jvboxid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool DeleteJVBox(int JVBoxId)
        {
            try
            {
                var DJVB=_jVBoxRepository.DeleteJVBox(JVBoxId);

                return DJVB;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        

        public IList<JVBoxListingViewModal> GetJVBoxData(string SearchCriteria, string PageSize, int PageNumber, int? StatusId)
        {
            var Model = _jVBoxRepository.GetJVBoxData(SearchCriteria, PageNumber, PageSize, StatusId);

            return Model;
        }

        public IList<JVBoxCustomerHistoryDTO> GetJVBoxCustomers(short JVBoxId)
        {
            var model = _jVBoxRepository.GetJVBoxCustomers(JVBoxId);
            return model;


        }
    }
}
