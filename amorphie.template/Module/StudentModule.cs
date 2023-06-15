using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.template.core.Model;
using amorphie.template.Validator;
using amorphie.core.Module.minimal_api;
using amorphie.template.data;

namespace amorphie.template.Module;

public sealed class StudentModule : BaseBBTRoute<Student, Student, TemplateDbContext>
{
    public StudentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "FirstMidName", "LastName" };

    public override string? UrlFragment => "student";


    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
    }
  
}
