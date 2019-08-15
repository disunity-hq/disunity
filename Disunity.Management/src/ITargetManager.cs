using System.Collections.Generic;
using System.Threading.Tasks;

using Disunity.Management.Models;


namespace Disunity.Management {

    public interface ITargetManager {

        Task<List<Target>> LoadAllTargets();
        
    }

}