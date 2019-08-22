namespace Disunity.Client.v1 {

    public interface IClient: IClientBase {

        IDisunityClient DisunityClient { get; }
        IModListClient ModListClient { get; }
        IModPublishingClient ModPublishingClient { get; }
        IOrgMemberClient OrgMemberClient{ get; }
        ITargetClient TargetClient { get; }
        IUploadClient UploadClient { get; }

    }

}