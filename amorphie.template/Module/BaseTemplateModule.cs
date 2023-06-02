using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;
using amorphie.core.Module.minimal_api;
using amorphie.core.Repository;
using FluentValidation;
using amorphie.template.data;

namespace amorphie.template.Module;
public abstract class BaseTemplateModule<TDTOModel, TDBModel, TValidator>
    : BaseBBTRouteRepository<TDTOModel, TDBModel, TValidator, TemplateDbContext, IBBTRepository<TDBModel, TemplateDbContext>>
    where TDTOModel : class, new()
    where TDBModel : EntityBase
    where TValidator : AbstractValidator<TDBModel>
{
    public BaseTemplateModule(WebApplication app) : base(app)
    {
    }

    public override string[]? PropertyCheckList => throw new NotImplementedException();

    public override string? UrlFragment => throw new NotImplementedException();
}
