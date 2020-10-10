import { action, observable, runInAction } from "mobx";
import agent from "../api/agent";
import { IProfile } from "../models/profile";
import { ISeriesForList, ISeriesLikedListItem, ISeriesWatchedListItem, IUserSeriesReview } from "../models/series";
import { RootStore } from "./rootStore";

export class ProfileStore {
    rootStore: RootStore;

    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;
    }


    @observable loadedProfile: IProfile | null = null;

    @observable watchedSeries: ISeriesWatchedListItem[] = [];
    @observable likedSeries: ISeriesLikedListItem[] = [];
    @observable reviews: IUserSeriesReview[] = [];

    @action clearProfileData = () => {
        this.loadedProfile = null;
        this.watchedSeries = [];
        this.likedSeries = [];
        this.reviews = [];
    }

    @action loadProfile = async (userId: string) => {
        var response = await agent.profile.get(userId);
        runInAction("loading user profile", async () => {
            if(response) {
                this.loadedProfile = response;
                this.loadWatchedSeries();
                this.loadLikedSeries();
                this.loadSeriesReviews();
            }
        }) 
    }

    @action loadWatchedSeries = async () => {
        if(this.loadedProfile) {
            var response = await agent.watching
                .getSeriesWatched(this.loadedProfile.userId);
            runInAction("loading watched series", () => {
                if(response) {
                    for (const series of response) {
                        this.watchedSeries.push(series);
                    }
                }
            })
        }
    }

    @action loadLikedSeries = async () => {
        if(this.loadedProfile) {
            var response = await agent.watching
                .getSeriesLiked(this.loadedProfile.userId);
            runInAction("loading liked series", () => {
                if(response) {
                    for (const series of response) {
                        this.likedSeries.push(series);
                    }
                }
            })
        }
    }

    @action loadSeriesReviews = async () => {
        if(this.loadedProfile) {
            var response = await agent.review
                .getUserReviews(this.loadedProfile.userId);
            runInAction("loading liked series", () => {
                if(response) {
                    for (const review of response) {
                        this.reviews.push(review);
                    }
                }
            })
        }
    }

}