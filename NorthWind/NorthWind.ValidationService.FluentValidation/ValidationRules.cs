using FluentValidation;
using NorthWind.Validation.Entities.Interfaces;
using System.Linq.Expressions;

namespace NorthWind.ValidationService.FluentValidation;

internal class ValidationRules<T, TProperty>(
    IRuleBuilderInitial<T, TProperty> ruleBuilderInitial)
    : IValidationRules<T, TProperty>
{
    // Utilizada para reglas sobre propiedades de tipo string
    IRuleBuilder<T, string> StringBuilder =>
        (IRuleBuilder<T, string>)ruleBuilderInitial;

    // Utilizada para devolver el resultado de una regla en
    // propiedades de tipo string.
    IValidationRules<T, string> ThisAsStringValidationRules =>
        this as IValidationRules<T, string>;

    public IValidationRules<T, TProperty> NotEmpty(string errorMessage)
    {
        ruleBuilderInitial.NotEmpty().WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> NotNull(string errorMessage)
    {
        ruleBuilderInitial.NotNull().WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> Must(
        Func<TProperty, bool> predicate, string errorMessage)
    {
        ruleBuilderInitial.Must(predicate).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> StopOnFirstFailure()
    {
        // FluentValidation no expone StopOnFirstFailure en IRuleBuilderInitial.
        // Se ignora por ahora (no lo usamos en esta práctica).
        return this;
    }

    public IValidationRules<T, TProperty> GreaterThan<TValue>(
        TValue valueToCompare, string errorMessage)
        where TValue : TProperty, IComparable<TValue>, IComparable
    {
        var Rule = ruleBuilderInitial as IRuleBuilder<T, TValue>;
        Rule.GreaterThan(valueToCompare).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> Equal(
        Expression<Func<T, TProperty>> expression, string errorMessage)
    {
        ruleBuilderInitial.Equal(expression).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, string> Length(int length, string errorMessage)
    {
        StringBuilder.Length(length).WithMessage(errorMessage);
        return ThisAsStringValidationRules;
    }

    public IValidationRules<T, string> MaximumLength(
        int length, string errorMessage)
    {
        StringBuilder.MaximumLength(length).WithMessage(errorMessage);
        return ThisAsStringValidationRules;
    }

    public IValidationRules<T, string> MinimumLength(
        int length, string errorMessage)
    {
        StringBuilder.MinimumLength(length).WithMessage(errorMessage);
        return ThisAsStringValidationRules;
    }

    public IValidationRules<T, string> EmailAddress(string errorMessage)
    {
        StringBuilder.EmailAddress().WithMessage(errorMessage);
        return ThisAsStringValidationRules;
    }
}