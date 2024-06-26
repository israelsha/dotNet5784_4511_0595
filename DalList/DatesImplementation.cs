﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;

namespace Dal;

internal class DatesImplementation : IDates
{
    public DateTime? getStartProject()
    {
        return DataSource.Config.startProjectDate;
    }

    public DateTime? setStartProject(DateTime? startProject)
    {
        DataSource.Config.startProjectDate = startProject;
        return startProject;
    }

    public DateTime? getEndProject()
    {
        return DataSource.Config.endProjectDate;
    }

    public DateTime? setEndProject(DateTime? endProject)
    {
        DataSource.Config.endProjectDate = endProject;
        return endProject;
    }

}
