//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusShedules.ADOModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class RouteStop
    {
        public int RouteStopId { get; set; }
        public int RouteId { get; set; }
        public int StopId { get; set; }
    
        public virtual Route Route { get; set; }
        public virtual Stop Stop { get; set; }
    }
}