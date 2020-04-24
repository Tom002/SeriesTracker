import { observable, action, runInAction } from "mobx";
import { AuthService } from "../api/auth";
import { User } from "oidc-client";
import { IProfile } from "../models/profile";
import { createContext } from "react";
import agent from "../api/agent";
import { IEpisodeWatched, IWatchSeriesRequest } from "../models/series";


class UserStore {

    authService = new AuthService();
    @observable currentUser : User | null = null;
    @observable userProfile : IProfile | null = null;
    @observable seriesWatched: number[] = [];
    @observable episodesWatched: number[] = [];

    @action loadUser = () => {
        this.authService.getUser().then(user => {
            if(user) {
                this.currentUser = user;
                
                agent.profile.get(user.profile.sub).then(profile => {
                    if(profile) {
                        runInAction(() => {
                            this.userProfile = profile;
                            console.log(profile);
                        })
                    }
                }, error => {
                    console.log(error);
                });
            }
        });
    }

    @action getSeriesWatched = async (userId: string) => {
        if(this.currentUser && !this.currentUser.expired) {
            let response = await agent.watching.listSeriesWatched(userId);
            runInAction(() => {
                if(response) {
                    this.seriesWatched.push(...response.seriesWatchedIds);
                }
            })
        }
    }

    @action getEpisodesWatchedForSeries = async (userId: string, seriesId: number) => {
        if(this.currentUser && !this.currentUser.expired) {
            let response = await agent.watching.listEpisodesWatched(userId, seriesId);
            runInAction(() => {
                if(response) {
                    for (const episodeId of response.episodesWatchedIds) {
                        this.episodesWatched.push(episodeId);
                    }
                    console.log("episodes watched:");
                    console.log(this.episodesWatched);
                }
            })
        }
    }

    @action watchSeries = async (userId: string, seriesId: number) => {
        console.log("watchhhhhhhhhhh");
        if(this.currentUser && !this.currentUser.expired) {
            let response = await agent.watching.watchSeries({viewerId:userId, seriesId:seriesId});
            runInAction(() =>{
                if(response) {
                    console.log(response);
                    this.seriesWatched.push(seriesId);
                }
            })
        }
    }

    @action watchEpisode = async (
        userId: string, 
        seriesId: number,
        episodeId: number,
        watchingDate: Date,
        addToDiary: boolean) => {
            if(this.currentUser && !this.currentUser.expired) {
                if(!this.episodesWatched.includes(episodeId)) {
                    let response = await agent.watching.watchEpisode(
                        {
                            viewerId: userId,
                            seriesId: seriesId,
                            episodeId: episodeId,
                            addToDiary: false,
                            watchingDate: watchingDate
                        });
                    runInAction(() => {
                        if(response) {
                            this.episodesWatched.push(episodeId);
                        }
                    })
                }
            }
        }




}

export default createContext(new UserStore());