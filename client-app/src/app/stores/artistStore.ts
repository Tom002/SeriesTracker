import { observable, action, runInAction } from "mobx";
import { IArtist } from "../models/artist";
import agent from "../api/agent";
import { IPagination } from "../models/searchParams";
import { createContext } from "react";


class ArtistStore {
    @observable artistRegistry = new Map<number, IArtist>();

    @observable currentPage = 1;
    @observable totalPages = 1;

    @action setCurrentPage = (page: number) => {
        runInAction("setting page", () => {
            console.log("page param:" + page);
            this.currentPage = page;
            console.log(this.currentPage);
        })
    }

    @action loadArtists = async(params: URLSearchParams) => {
        this.artistRegistry.clear();
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
}
export default createContext(new ArtistStore());