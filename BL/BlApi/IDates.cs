using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IDates
{
    public DateTime? setStartProject(DateTime? startProject);
    public DateTime? getStartProject();

    public void resetDate(DateTime startProject);



}
