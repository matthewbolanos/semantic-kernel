﻿// Copyright (c) Microsoft. All rights reserved.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(0)
        .AddDebug();
});

// Create kernel
IKernel kernel = new KernelBuilder()
    // Add a text or chat completion service using either:
    // .WithAzureTextCompletionService()
    // .WithAzureChatCompletionService()
    // .WithOpenAITextCompletionService()
    // .WithOpenAIChatCompletionService()
    .WithCompletionService()
    .WithLoggerFactory(loggerFactory)
    .Build();

// Add the math plugin
var mathPlugin = kernel.ImportSkill(new Plugins.MathPlugin.Math(), "MathPlugin");

// Create a planner
var planner = new SequentialPlanner(kernel);

var ask = "If my investment of 2130.23 dollars increased by 23%, how much would I have after I spent $5 on a latte?";
var plan = await planner.CreatePlanAsync(ask);

Console.WriteLine("Plan:\n");
Console.WriteLine(JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }));

var result = (await kernel.RunAsync(plan)).Result;

Console.WriteLine("Plan results:");
Console.WriteLine(result.Trim());
