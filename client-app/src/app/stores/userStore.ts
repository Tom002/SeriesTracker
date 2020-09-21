import { observable, action, runInAction } from "mobx";
import { AuthService } from "../api/auth";
import { User } from "oidc-client";
import { IProfile } from "../models/profile";
import { createContext } from "react";
import agent from "../api/agent";
import {
  IEpisodeWatched,
  IWatchSeriesRequest,
  IReviewSeriesRequest,
  IUserSeriesReviewInfo,
} from "../models/series";

class UserStore {
  authService = new AuthService();
  @observable currentUser: User | null = null;
  @observable userProfile: IProfile | null = null;
  @observable seriesWatched: number[] = [];
  @observable episodesWatched: number[] = [];
  @observable selectedSeriesReview: IUserSeriesReviewInfo | null = null;

  @action loadUser = () => {
    this.authService.getUser().then((user) => {
      if (user) {
        this.currentUser = user;

        agent.profile.get(user.profile.sub).then(
          (profile) => {
            if (profile) {
              runInAction(() => {
                this.userProfile = profile;
                console.log(profile);
              });
            }
          },
          (error) => {
            console.log(error);
          }
        );
      }
    });
  };

  @action getSeriesWatched = async (userId: string) => {
    if (this.currentUser && !this.currentUser.expired) {
      let response = await agent.watching.listSeriesWatched(userId);
      runInAction(() => {
        if (response) {
          this.seriesWatched.push(...response.seriesWatchedIds);
        }
      });
    }
  };

  @action getEpisodesWatchedForSeries = async (
    userId: string,
    seriesId: number
  ) => {
    if (this.currentUser && !this.currentUser.expired) {
      let response = await agent.watching.listEpisodesWatched(userId, seriesId);
      runInAction(() => {
        if (response) {
          for (const episodeId of response.episodesWatchedIds) {
            this.episodesWatched.push(episodeId);
          }
          console.log("episodes watched:");
          console.log(this.episodesWatched);
        }
      });
    }
  };

  @action getSelectedSeriesReview = async (
    userId: string,
    seriesId: number
  ) => {
    let response = await agent.review.getUserReview(userId, seriesId);
    if (response) {
      runInAction(() => {
        this.selectedSeriesReview = response;
      });
    }
  };

  @action cleanSelectedSeriesReview = () => {
    runInAction(() => {
      console.log("clean selected series review");
      this.selectedSeriesReview = null;
    });
  };

  @action reviewSeries = async (
    userId: string,
    seriesId: number,
    rating: number
  ) => {
    if (this.currentUser && !this.currentUser.expired) {
      let request: IReviewSeriesRequest = {
        rating: rating,
        seriesId: seriesId,
        reviewerId: userId,
        reviewText: null,
        reviewTitle: null,
        reviewDate: null,
      };
      let response = await agent.review.reviewSeries(request);
      runInAction(() => {
        if (response) {
          if (this.selectedSeriesReview) {
            this.selectedSeriesReview.isReviewedByUser = true;
            this.selectedSeriesReview.rating = rating;
          } else {
            this.selectedSeriesReview = {
              rating: rating,
              seriesId: seriesId,
              reviewerId: userId,
              isReviewedByUser: true,
              reviewDate: new Date(),
              reviewText: null,
              reviewTitle: null,
            };
          }
        }
      });
    }
  };

  @action watchSeries = async (userId: string, seriesId: number) => {
    if (this.currentUser && !this.currentUser.expired) {
      let response = await agent.watching.watchSeries({
        viewerId: userId,
        seriesId: seriesId,
      });
      runInAction(() => {
        if (response) {
          console.log(response);
          this.seriesWatched.push(seriesId);
        }
      });
    }
  };

  @action watchEpisode = async (
    userId: string,
    seriesId: number,
    episodeId: number,
    watchingDate: Date,
    addToDiary: boolean
  ) => {
    if (this.currentUser && !this.currentUser.expired) {
      if (!this.episodesWatched.includes(episodeId)) {
        let response = await agent.watching.watchEpisode({
          viewerId: userId,
          seriesId: seriesId,
          episodeId: episodeId,
          addToDiary: false,
          watchingDate: watchingDate,
        });
        runInAction(() => {
          this.episodesWatched.push(episodeId);
        });
      }
    }
  };
}

export default createContext(new UserStore());
