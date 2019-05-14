import { Injectable } from '@angular/core';
import { IpcService } from '@services/ipc.service';
import { ProviderModel, MessageType, Helpers, BaseService } from '@common/index';

@Injectable()
export class DropboxService extends BaseService {

    constructor(private readonly _ipc: IpcService) {
        super();
    }

    protected Dispose(): void {
        this._ipc.Receive.unsubscribe();
    }

    async GetProvider(): Promise<ProviderModel> {
        let response = await this._ipc.SendAsync(MessageType.DropboxProvider);
        let provider = Helpers.Deserialize<ProviderModel>(response.DataJson, ProviderModel);
        return provider;
    }
}