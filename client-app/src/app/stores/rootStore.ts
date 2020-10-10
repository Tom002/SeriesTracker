import UserStore from './userStore';
import SeriesStore from './seriesStore';
import { createContext } from 'react';
import userStore from './userStore';
import seriesStore from './seriesStore';
import { ProfileStore } from './profileStore';
import { ArtistStore } from './artistStore';

export class RootStore {

    userStore: UserStore;
    artistStore: ArtistStore;
    seriesStore: SeriesStore;
    profileStore: ProfileStore;

    constructor() {
        this.userStore = new UserStore(this)
        this.seriesStore = new SeriesStore(this)
        this.profileStore = new ProfileStore(this);
        this.artistStore = new ArtistStore(this);
    }
}

export default createContext(new RootStore());