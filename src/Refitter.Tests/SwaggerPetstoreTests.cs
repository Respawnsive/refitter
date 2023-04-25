﻿using FluentAssertions;
using Refitter.Core;
using Refitter.Tests.Build;
using Refitter.Tests.Resources;
using Xunit;

namespace Refitter.Tests;

public class SwaggerPetstoreTests
{
    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code(SampleOpenSpecifications version, string filename)
    {
        var generateCode = await GenerateCode(version, filename);
        generateCode.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_Without_Contracts(SampleOpenSpecifications version, string filename)
    {
        var generateCode = await GenerateCode(
            version,
            filename,
            new RefitGeneratorSettings { GenerateXmlDocCodeComments = false, GenerateContracts = false });
        generateCode.Should().NotContain("class Pet");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_Without_XmlDoc_Code_Comments(SampleOpenSpecifications version, string filename)
    {
        var generateCode = await GenerateCode(
            version,
            filename,
            new RefitGeneratorSettings { GenerateXmlDocCodeComments = false, GenerateContracts = false });
        generateCode.Should().NotContain("<summary>");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_With_Default_Namespace(SampleOpenSpecifications version, string filename)
    {
        var generateCode = await GenerateCode(
            version,
            filename,
            new RefitGeneratorSettings { Namespace = "Some.Other.Namespace" });
        generateCode.Should().Contain("namespace Some.Other.Namespace");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_With_Fixed_Interface_Name(SampleOpenSpecifications version, string filename)
    {
        var settings = new RefitGeneratorSettings();
        settings.Naming.UseOpenApiTitle = false;
        settings.Naming.InterfaceName = "SomeOtherName";
        var generateCode = await GenerateCode(version, filename,settings);
        generateCode.Should().Contain("interface ISomeOtherName");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_Without_AutoGenerated_Header(SampleOpenSpecifications version, string filename)
    {
        var settings = new RefitGeneratorSettings();
        settings.AddAutoGeneratedHeader = false;
        var generateCode = await GenerateCode(version, filename, settings);
        generateCode.Should().NotContain("This code was generated by Refitter");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_That_Returns_IApiResponse(SampleOpenSpecifications version, string filename)
    {
        var settings = new RefitGeneratorSettings();
        settings.ReturnIApiResponse = true;
        var generateCode = await GenerateCode(version, filename, settings);
        generateCode.Should().Contain("Task<IApiResponse<Pet>>");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_With_Internal_Contract_Types(SampleOpenSpecifications version, string filename)
    {
        var settings = new RefitGeneratorSettings();
        settings.TypeAccessibility = TypeAccessibility.Internal;
        var generateCode = await GenerateCode(version, filename, settings);
        generateCode.Should().Contain("internal partial class");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_With_Internal_Interface(SampleOpenSpecifications version, string filename)
    {
        var settings = new RefitGeneratorSettings();
        settings.TypeAccessibility = TypeAccessibility.Internal;
        var generateCode = await GenerateCode(version, filename, settings);
        generateCode.Should().Contain("internal interface");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
    public async Task Can_Generate_Code_With_CancellationToken(SampleOpenSpecifications version, string filename)
    {
        var settings = new RefitGeneratorSettings();
        settings.UseCancellationTokens = true;
        var generateCode = await GenerateCode(version, filename, settings);
        generateCode.Should().Contain("CancellationToken cancellationToken = default");
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
#if !DEBUG
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
#endif
    public async Task Can_Build_Generated_Code(SampleOpenSpecifications version, string filename)
    {
        var generateCode = await GenerateCode(version, filename);
        BuildHelper
            .BuildCSharp(generateCode)
            .Should()
            .BeTrue();
    }

    [Theory]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV3, "SwaggerPetstore.json")]
#if !DEBUG
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV3, "SwaggerPetstore.yaml")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreJsonV2, "SwaggerPetstore.json")]
    [InlineData(SampleOpenSpecifications.SwaggerPetstoreYamlV2, "SwaggerPetstore.yaml")]
#endif
    public async Task Can_Build_Generated_Code_With_IApiResponse(SampleOpenSpecifications version, string filename)
    {
        var settings = new RefitGeneratorSettings();
        settings.ReturnIApiResponse = true;
        var generateCode = await GenerateCode(version, filename, settings);
        BuildHelper
            .BuildCSharp(generateCode)
            .Should()
            .BeTrue();
    }

    private static async Task<string> GenerateCode(
        SampleOpenSpecifications version,
        string filename,
        RefitGeneratorSettings? settings = null)
    {
        var swaggerFile = await CreateSwaggerFile(EmbeddedResources.GetSwaggerPetstore(version), filename);
        if (settings is null)
        {
            settings = new RefitGeneratorSettings { OpenApiPath = swaggerFile };
        }
        else
        {
            settings.OpenApiPath = swaggerFile;
        }

        var sut = await RefitGenerator.CreateAsync(settings);
        return sut.Generate();
    }

    private static async Task<string> CreateSwaggerFile(string contents, string filename)
    {
        var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(folder);
        var swaggerFile = Path.Combine(folder, filename);
        await File.WriteAllTextAsync(swaggerFile, contents);
        return swaggerFile;
    }
}