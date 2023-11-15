using FluentValidation;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Helpers.TestHelpers;

public class FluentValidationTestHelper<TValidator, TModel>
    where TValidator : AbstractValidator<TModel>
    where TModel : class
{
    private readonly TValidator _validator;

    public FluentValidationTestHelper()
    {
        _validator = Activator.CreateInstance<TValidator>();
    }

    public void TestEmailFieldForSuccessCases<TProperty>(Expression<Func<TModel, TProperty>> expression, TModel model)
    {
        var successCases = new List<string>() { "cilgincasevenasik@outlook.com", "cilgincasevenasik@msn.com" };

        RunAllSuccessTestScenarios(expression, model, successCases);
    }

    public void TestEmailFieldForErrorCases<TProperty>(Expression<Func<TModel, TProperty>> expression, TModel model, string errorMessage)
    {
        var errorCases = new List<string?>() { "cilgincasevenasik", "cilgincasevenasik@", "@outlook.com", "" ,
        null,  "missing.at.com",  "multiple@@at.com", "missingdomain@"};

        RunAllErrorTestScenarios(expression, model, errorMessage, errorCases);
    }

    public void TestIdentityNoFieldForSuccessCases<TProperty>(Expression<Func<TModel, TProperty>> expression, TModel model)
    {
        var successCases = new List<string?>() { "87432267324", "52329508712", "22842485982" };

        RunAllSuccessTestScenarios(expression, model, successCases);
    }

    public void TestIdentityNoFieldForErrorCases<TProperty>(Expression<Func<TModel, TProperty>> expression, TModel model, string errorMessage)
    {
        var errorCases = new List<string?>() { "72053235211", "874322673244", "8743226732", "", null };

        RunAllErrorTestScenarios(expression, model, errorMessage, errorCases);
    }

    private void RunAllErrorTestScenarios<TProperty, TCaseType>(Expression<Func<TModel, TProperty>> expression, TModel model, string errorMessage, List<TCaseType> errorCases)
    {
        foreach (TCaseType errorCase in errorCases)
        {
            SetPropertyValue(expression, model, errorCase);
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(expression).WithErrorMessage(errorMessage);
        }
    }

    private void RunAllSuccessTestScenarios<TProperty, TCaseType>(Expression<Func<TModel, TProperty>> expression, TModel model, List<TCaseType> successCases)
    {
        foreach (TCaseType errorCase in successCases)
        {
            SetPropertyValue(expression, model, errorCase);
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(expression);
        }
    }

    private void SetPropertyValue<TProperty, TPropertyType>(Expression<Func<TModel, TProperty>> expression, TModel model, TPropertyType newValue)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            var propertyInfo = memberExpression.Member as PropertyInfo ?? throw new ArgumentException("The expression must specify a property.");

            if (propertyInfo.PropertyType != typeof(TPropertyType))
                throw new ArgumentException($"Property '{propertyInfo.Name}' has a different type than the provided value.");

            propertyInfo.SetValue(model, newValue);
        }
        else throw new ArgumentException("The expression must be a simple property access expression.");
    }
}