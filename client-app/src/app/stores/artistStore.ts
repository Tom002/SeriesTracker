import { observable, action, runInAction } from "mobx";
import { IArtist, IArtistDetails } from "../models/artist";
import agent from "../api/agent";
import { IPagination } from "../models/searchParams";
import { createContext } from "react";
import { RootStore } from "./rootStore";


export class ArtistStore {

    rootStore: RootStore;
    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;
    }

    @observable artistRegistry = new Map<number, IArtist>();
    @observable selectedArtist: IArtistDetails | null = null;
    @observable currentPage = 1;
    @observable totalPages = 1;

    @action setCurrentPage = (page: number) => {
        runInAction("setting page", () => {
            console.log("page param:" + page);
            this.currentPage = page;
            console.log(this.currentPage);
        })
    }

    @action clearArtistRegistry = () => {
        runInAction("clearing artist registry", () => {
            this.artistRegistry.clear();
        })
    }

    @action clearSelectedArtist = () => {
        runInAction("clearing selected artist", () => {
            this.selectedArtist = null;
        })
    }

    @action loadArtists = async(params: URLSearchParams) => {
        this.clearArtistRegistry();
        try {
            const response = await agent.artists.list(params);
            const pagination : IPagination = JSON.parse(response.headers.pagination);
            runInAction(() => {
                this.currentPage = pagination.currentPage;
                this.totalPages = pagination.totalPages;
                for (const artist of response.data) {
                    this.artistRegistry.set(artist.artistId, artist);
                }
            })

        } catch (error) {
            console.log(error);
        }
    }

    @action loadSingleArtist = async(artistId: number) => {
        this.clearSelectedArtist();
        var artist = await agent.artists.get(artistId);
        runInAction("loading selected artist", () => {
            if(artist) {
                this.selectedArtist = artist;
            }
        })
    }
}