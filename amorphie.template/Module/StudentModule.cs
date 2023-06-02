using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.template.core.Model;
using amorphie.template.Validator;

namespace amorphie.template.Module;

public sealed class StudentModule : BaseTemplateModule<Student, Student, StudentValidator>
{
    public StudentModule(WebApplication app) : base(app)
    {
    }

    public override string[]? PropertyCheckList => new string[] { "FirstMidName","LastName" };

    public override string? UrlFragment => "student";
}
