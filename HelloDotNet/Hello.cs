﻿using System;
using Common.Logging;
using LaunchDarkly.Client;
using LaunchDarkly.Logging;

namespace HelloDotNet
{
    class Hello
    {
        // Set SdkKey to your LaunchDarkly SDK key.
        public const string SdkKey = "";

        // Set FeatureFlagKey to the feature flag key you want to evaluate.
        public const string FeatureFlagKey = "my-boolean-flag";

        private static void ShowMessage(string s) {
            Console.WriteLine("*** " + s);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(SdkKey))
            {
                ShowMessage("Please edit Hello.cs to set SdkKey to your LaunchDarkly SDK key first");
                Environment.Exit(1);
            }

            LogManager.Adapter = new ConsoleAdapter(LogLevel.Info);
            // ConsoleAdapter is a simple Common.Logging implementation that's provided by LaunchDarkly;
            // Common.Logging has an equivalent class, but not on every platform

            Configuration ldConfig = LaunchDarkly.Client.Configuration
                .Default(SdkKey);

            LdClient client = new LdClient(ldConfig);

            if (client.Initialized())
            {
                ShowMessage("SDK successfully initialized!");
            }
            else
            {
                ShowMessage("SDK failed to initialize");
                Environment.Exit(1);
            }

            // Set up the user properties. This user should appear on your LaunchDarkly users dashboard
            // soon after you run the demo.
            User user = User.Builder("example-user-key")
                .Name("Sandy")
                .Build();

            var flagValue = client.BoolVariation(FeatureFlagKey, user, false);

            ShowMessage(string.Format("Feature flag '{0}' is {1} for this user",
                FeatureFlagKey, flagValue));

            // Here we ensure that the SDK shuts down cleanly and has a chance to deliver analytics
            // events to LaunchDarkly before the program exits. If analytics events are not delivered,
            // the user properties and flag usage statistics will not appear on your dashboard. In a
            // normal long-running application, the SDK would continue running and events would be
            // delivered automatically in the background.
            client.Dispose();
        }
    }
}
