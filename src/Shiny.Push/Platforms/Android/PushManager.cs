﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Android.Gms.Extensions;
using Firebase.Iid;
using Firebase.Messaging;
using Shiny.Settings;
using Task = System.Threading.Tasks.Task;
using CancellationToken = System.Threading.CancellationToken;


namespace Shiny.Push
{
    public class PushManager : AbstractPushManager, IPushTagSupport
    {
        readonly AndroidContext context;
        readonly IMessageBus bus;


        public PushManager(AndroidContext context,
                           ISettings settings,
                           IMessageBus bus) : base(settings)
        {
            this.context = context;
            this.bus = bus;
        }


        public override Task<PushAccessState> RequestAccess(CancellationToken cancelToken = default) => this.RequestAccess(null, cancelToken);
        public virtual async Task<PushAccessState> RequestAccess(string[] tags, CancellationToken cancelToken = default)
        {
            //var resultCode = GoogleApiAvailability
            //    .Instance
            //    .IsGooglePlayServicesAvailable(this.context.AppContext);

            //if (resultCode == ConnectionResult.)
            //if (resultCode != ConnectionResult.ServiceMissing)
            //{
            ////{
            ////    if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
            ////        msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
            //}
            var result = await FirebaseInstanceId.Instance.GetInstanceId().AsAsync<IInstanceIdResult>();

            this.CurrentRegistrationToken = result.Token;
            this.CurrentRegistrationTokenDate = DateTime.UtcNow;
            FirebaseMessaging.Instance.AutoInitEnabled = true;

            await this.UpdateTags(tags);

            return new PushAccessState(AccessState.Available, this.CurrentRegistrationToken);
        }


        public override async Task UnRegister()
        {
            this.ClearRegistration();
            FirebaseMessaging.Instance.AutoInitEnabled = false;

            // must be executed off proc
            await Task.Run(() => FirebaseInstanceId.Instance.DeleteInstanceId());
        }


        public override IObservable<IDictionary<string, string>> WhenReceived()
            => this.bus.Listener<IDictionary<string, string>>(nameof(ShinyFirebaseService));


        public async Task UpdateTags(params string[] tags)
        {
            if (this.RegisteredTags != null)
            { 
                foreach (var tag in this.RegisteredTags)
                {
                    await FirebaseMessaging.Instance.UnsubscribeFromTopic(tag);
                }
            }
            if (tags != null)
            { 
                foreach (var tag in tags)
                {
                    await FirebaseMessaging.Instance.SubscribeToTopic(tag);
                }
            }
            this.RegisteredTags = tags;
        }
    }
}
