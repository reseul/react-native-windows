// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ReactNative.Bridge;
using ReactNative.Bridge.Queue;
using ReactNative.Modules.Core;
using ReactNative.UIManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ReactNative.Tests.UIManager
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class UIManagerModuleTests
    {

        [Test]
        public void UIManagerModule_ArgumentChecks()
        {
            var context = new ReactContext();
            var viewManagers = new List<IViewManager>();
            var uiImplementationProvider = new UIImplementationProvider();

            using (var actionQueue = new ActionQueue(ex => { }))
            {
                ArgumentNullException ex1 = Assert.Throws<ArgumentNullException>(
                    () => new UIManagerModule(context, null, uiImplementationProvider, actionQueue, UIManagerModuleOptions.None));
                Assert.AreEqual("viewManagers", ex1.ParamName);

                ArgumentNullException ex2 = Assert.Throws<ArgumentNullException>(
                    () => new UIManagerModule(context, viewManagers, null, actionQueue, UIManagerModuleOptions.None));
                Assert.AreEqual("uiImplementationProvider", ex2.ParamName);

                ArgumentNullException ex3 = Assert.Throws<ArgumentNullException>(
                    () => new UIManagerModule(context, viewManagers, uiImplementationProvider, null, UIManagerModuleOptions.None));
                Assert.AreEqual("layoutActionQueue", ex3.ParamName);
            }
        }

        [Test]
        public async Task UIManagerModule_CustomEvents_Constants()
        {
            var context = new ReactContext();
            var viewManagers = new List<IViewManager> { new NoEventsViewManager() };

            ReactNative.Bridge.DispatcherHelpers.MainDispatcher = Dispatcher.CurrentDispatcher;
            await DispatcherHelpers.RunOnDispatcherAsync(ReactChoreographer.Initialize);

            var uiImplementationProvider = new UIImplementationProvider();
            using (var actionQueue = new ActionQueue(ex => { }))
            {
                var module = await DispatcherHelpers.CallOnDispatcherAsync(
                    () => new UIManagerModule(context, viewManagers, uiImplementationProvider, actionQueue, UIManagerModuleOptions.None));

                var constants = ((INativeModule)module).Constants;

                Assert.AreEqual("onSelect", constants.GetMap("genericBubblingEventTypes").GetMap("topSelect").GetMap("phasedRegistrationNames").GetValue("bubbled").Value<string>());
                Assert.AreEqual("onSelectCapture", constants.GetMap("genericBubblingEventTypes").GetMap("topSelect").GetMap("phasedRegistrationNames").GetValue("captured").Value<string>());
                Assert.AreEqual("onChange", constants.GetMap("genericBubblingEventTypes").GetMap("topChange").GetMap("phasedRegistrationNames").GetValue("bubbled").Value<string>());
                Assert.AreEqual("onChangeCapture", constants.GetMap("genericBubblingEventTypes").GetMap("topChange").GetMap("phasedRegistrationNames").GetValue("captured").Value<string>());
                Assert.AreEqual("onTouchStart", constants.GetMap("genericBubblingEventTypes").GetMap("topTouchStart").GetMap("phasedRegistrationNames").GetValue("bubbled").Value<string>());
                Assert.AreEqual("onTouchStartCapture", constants.GetMap("genericBubblingEventTypes").GetMap("topTouchStart").GetMap("phasedRegistrationNames").GetValue("captured").Value<string>());
                Assert.AreEqual("onTouchMove", constants.GetMap("genericBubblingEventTypes").GetMap("topTouchMove").GetMap("phasedRegistrationNames").GetValue("bubbled").Value<string>());
                Assert.AreEqual("onTouchMoveCapture", constants.GetMap("genericBubblingEventTypes").GetMap("topTouchMove").GetMap("phasedRegistrationNames").GetValue("captured").Value<string>());
                Assert.AreEqual("onTouchEnd", constants.GetMap("genericBubblingEventTypes").GetMap("topTouchEnd").GetMap("phasedRegistrationNames").GetValue("bubbled").Value<string>());
                Assert.AreEqual("onTouchEndCapture", constants.GetMap("genericBubblingEventTypes").GetMap("topTouchEnd").GetMap("phasedRegistrationNames").GetValue("captured").Value<string>());
                Assert.AreEqual("onMouseOver", constants.GetMap("genericBubblingEventTypes").GetMap("topMouseOver").GetMap("phasedRegistrationNames").GetValue("bubbled").Value<string>());
                Assert.AreEqual("onMouseOverCapture", constants.GetMap("genericBubblingEventTypes").GetMap("topMouseOver").GetMap("phasedRegistrationNames").GetValue("captured").Value<string>());
                Assert.AreEqual("onMouseOut", constants.GetMap("genericBubblingEventTypes").GetMap("topMouseOut").GetMap("phasedRegistrationNames").GetValue("bubbled").Value<string>());
                Assert.AreEqual("onMouseOutCapture", constants.GetMap("genericBubblingEventTypes").GetMap("topMouseOut").GetMap("phasedRegistrationNames").GetValue("captured").Value<string>());

                Assert.AreEqual("onSelectionChange", constants.GetMap("genericDirectEventTypes").GetMap("topSelectionChange").GetValue("registrationName").Value<string>());
                Assert.AreEqual("onLoadingStart", constants.GetMap("genericDirectEventTypes").GetMap("topLoadingStart").GetValue("registrationName").Value<string>());
                Assert.AreEqual("onLoadingFinish", constants.GetMap("genericDirectEventTypes").GetMap("topLoadingFinish").GetValue("registrationName").Value<string>());
                Assert.AreEqual("onLoadingError", constants.GetMap("genericDirectEventTypes").GetMap("topLoadingError").GetValue("registrationName").Value<string>());
                Assert.AreEqual("onLayout", constants.GetMap("genericDirectEventTypes").GetMap("topLayout").GetValue("registrationName").Value<string>());
                Assert.AreEqual("onMouseEnter", constants.GetMap("genericDirectEventTypes").GetMap("topMouseEnter").GetValue("registrationName").Value<string>());
                Assert.AreEqual("onMouseLeave", constants.GetMap("genericDirectEventTypes").GetMap("topMouseLeave").GetValue("registrationName").Value<string>());
                Assert.AreEqual("onMessage", constants.GetMap("genericDirectEventTypes").GetMap("topMessage").GetValue("registrationName").Value<string>());
            }

            // Ideally we should dispose, but the original dispatcher is somehow lost/etc.
            // await DispatcherHelpers.RunOnDispatcherAsync(ReactChoreographer.Dispose);
        }

        [Test]
        public async Task UIManagerModule_Constants_ViewManager_CustomEvents()
        {
            var context = new ReactContext();
            var viewManagers = new List<IViewManager> { new TestViewManager() };

            ReactNative.Bridge.DispatcherHelpers.MainDispatcher = Dispatcher.CurrentDispatcher;
            await DispatcherHelpers.RunOnDispatcherAsync(ReactChoreographer.Initialize);

            var uiImplementationProvider = new UIImplementationProvider();
            using (var actionQueue = new ActionQueue(ex => { }))
            {
                var module = await DispatcherHelpers.CallOnDispatcherAsync(
                    () => new UIManagerModule(context, viewManagers, uiImplementationProvider, actionQueue, UIManagerModuleOptions.None));

                var constants = ((INativeModule)module).Constants.GetMap("Test");
                Assert.AreEqual(42, constants.GetMap("directEventTypes").GetValue("otherSelectionChange").Value<int>());
                Assert.AreEqual(42, constants.GetMap("directEventTypes").GetMap("topSelectionChange").GetValue("registrationName").Value<int>());
                Assert.AreEqual(42, constants.GetMap("directEventTypes").GetMap("topLoadingStart").GetValue("foo").Value<int>());
                Assert.AreEqual(42, constants.GetMap("directEventTypes").GetValue("topLoadingError").Value<int>());
            }

            // Ideally we should dispose, but the original dispatcher is somehow lost/etc.
            // await DispatcherHelpers.RunOnDispatcherAsync(ReactChoreographer.Dispose);
        }

        [Test]
        public async Task UIManagerModule_Constants_ViewManager_LazyConstants()
        {
            var context = new ReactContext();
            var viewManagers = new List<IViewManager> { new TestViewManager() };

            ReactNative.Bridge.DispatcherHelpers.MainDispatcher = Dispatcher.CurrentDispatcher;
            await DispatcherHelpers.RunOnDispatcherAsync(ReactChoreographer.Initialize);

            var uiImplementationProvider = new UIImplementationProvider();
            using (var actionQueue = new ActionQueue(ex => { }))
            {
                var module = await DispatcherHelpers.CallOnDispatcherAsync(
                    () => new UIManagerModule(context, viewManagers, uiImplementationProvider, actionQueue, UIManagerModuleOptions.LazyViewManagers));

                var obj = ((INativeModule)module).Constants.GetValue("ViewManagerNames");
                var viewManagerNames = obj as JArray;
                Assert.IsNotNull(viewManagerNames);
                Assert.AreEqual(1, viewManagerNames.Count());
                Assert.AreEqual("Test", viewManagerNames.Single().Value<string>());
            }

            // Ideally we should dispose, but the original dispatcher is somehow lost/etc.
            // await DispatcherHelpers.RunOnDispatcherAsync(ReactChoreographer.Dispose);
        }

        [Test]
        public async Task UIManagerModule_getConstantsForViewManager()
        {
            var context = new ReactContext();
            var viewManagers = new List<IViewManager> { new TestViewManager() };

            ReactNative.Bridge.DispatcherHelpers.MainDispatcher = Dispatcher.CurrentDispatcher;
            await DispatcherHelpers.RunOnDispatcherAsync(ReactChoreographer.Initialize);

            var uiImplementationProvider = new UIImplementationProvider();
            using (var actionQueue = new ActionQueue(ex => { }))
            {
                var module = await DispatcherHelpers.CallOnDispatcherAsync(
                    () => new UIManagerModule(context, viewManagers, uiImplementationProvider, actionQueue, UIManagerModuleOptions.LazyViewManagers));

                var constants = module.getConstantsForViewManager("Test");
                Assert.AreEqual(42, constants.GetMap("directEventTypes").GetValue("otherSelectionChange").Value<int>());
                Assert.AreEqual(42, constants.GetMap("directEventTypes").GetMap("topSelectionChange").GetValue("registrationName").Value<int>());
                Assert.AreEqual(42, constants.GetMap("directEventTypes").GetMap("topLoadingStart").GetValue("foo").Value<int>());
                Assert.AreEqual(42, constants.GetMap("directEventTypes").GetValue("topLoadingError").Value<int>());
            }

            // Ideally we should dispose, but the original dispatcher is somehow lost/etc.
            // await DispatcherHelpers.RunOnDispatcherAsync(ReactChoreographer.Dispose);
        }

        class NoEventsViewManager : MockViewManager
        {
            public override JObject CommandsMap
            {
                get
                {
                    return null;
                }
            }

            public override JObject ExportedCustomBubblingEventTypeConstants
            {
                get
                {
                    return null;
                }
            }

            public override JObject ExportedCustomDirectEventTypeConstants
            {
                get
                {
                    return null;
                }
            }

            public override JObject ExportedViewConstants
            {
                get
                {
                    return null;
                }
            }

            public override JObject NativeProps
            {
                get
                {
                    return null;
                }
            }

            public override string Name
            {
                get
                {
                    return "Test";
                }
            }

            public override Type ShadowNodeType
            {
                get
                {
                    return typeof(ReactShadowNode);
                }
            }
        }

        class TestViewManager : NoEventsViewManager
        {
            public override JObject ExportedCustomDirectEventTypeConstants
            {
                get
                {
                    return new JObject
                    {
                        { "otherSelectionChange", 42 },
                        { "topSelectionChange", new JObject { { "registrationName", 42 } } },
                        { "topLoadingStart", new JObject { { "foo", 42 } } },
                        { "topLoadingError", 42 },
                    };
                }
            }
        }
    }
}
