import {observable, action, runInAction, computed} from 'mobx';
import { ISeriesForList, ICategory, ISeriesDetails, IEpisode, IUserSeriesReviewInfo, IReview, ISeriesWatchedInfo, ISeriesReviewRequest, ISeriesRateRequest } from '../models/series';
import agent from '../api/agent';
import { DropdownItemProps } from 'semantic-ui-react';
import { IPagination } from '../models/searchParams';
import { RootStore } from './rootStore';

class SeriesStore {

    rootStore: RootStore;
    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;
    }

    @observable seriesRegistry = new Map<number, ISeriesForList>();
    @observable categoryRegistry = new Map<number, ICategory>();
    
    @observable selectedSeries: ISeriesDetails | null = null;
    @observable selectedSeriesUserReview: IUserSeriesReviewInfo | null = null;
    @observable selectedSeriesWatchedInfo: ISeriesWatchedInfo = {
        isWatchingSeries: false,
        hasLikedSeries: false,
        episodesWatchedIdList: []
    }   
    
    @observable loadedEpisodes: IEpisode[] = [];
    @observable currentPage = 1;
    @observable totalPages = 1;
    
    @action setCurrentPage = (page: number) => {
        runInAction("setting page", () => {
            this.currentPage = page;
        })
    };

    @action clearSeriesRegistry = () => {
        runInAction("clearing series registry", () => {
            this.seriesRegistry.clear();
        })
    };

    @action clearCategoryRegistry = () => {
        runInAction("clearing category registry", () => {
            this.categoryRegistry.clear();
        })
    };

    @action clearLoadedEpisodes = () => {
        runInAction("clearing loaded episodes", () => {
            this.loadedEpisodes = [];
        })
    };

    @action clearSelectedSeries = () => {
        runInAction("clearing selected series", () => {
            this.selectedSeries = null;
            this.selectedSeriesUserReview = null;
            this.selectedSeriesWatchedInfo = {
                isWatchingSeries: false,
                hasLikedSeries: false,
                episodesWatchedIdList: []
            };
        })
    };
    
    @action setTotalPages = (page: number) => {
        runInAction("setting total pages", () => {
            this.totalPages = page;
        })
    };

    @action getSingleSeries = async (id: number) => {
        try {
            let series = await agent.series.get(id);
            if(series) {
                this.selectedSeries = series;
                await this.loadReviewListForSelectedSeries();
                console.log(this.selectedSeries.reviews);
                if(this.rootStore.userStore.currentUser && 
                   !this.rootStore.userStore.currentUser.expired) {
                       await this.loadUserReviewForSelectedSeries();
                       await this.loadUserWatchedInfoForSelectedSeries();
                   }
            }
        } catch (error) {
            console.log(error);
        }
    };
  
    @action loadSeries = async (params: URLSearchParams) => {
        this.clearSeriesRegistry();
        try {
            const response = await agent.series.list(params);
            console.log(response);
            const pagination : IPagination = JSON.parse(response.headers.pagination);
            this.setTotalPages(pagination.totalPages);
            this.setCurrentPage(pagination.currentPage);
            runInAction("loading series", () => {
                for (const series of response.data) {
                    this.seriesRegistry.set(series.seriesId, series);
                }
            });
        }
        catch(error) {
            console.log(error);
        }
    }

    @action loadUserReviewForSelectedSeries = async () => {
        if(this.selectedSeries) {
            var userReviewInfo = await agent.review.getMyReview(this.selectedSeries.seriesId);
            if(userReviewInfo) {
                runInAction("loading selected series review", () => {
                    this.selectedSeriesUserReview = userReviewInfo;
                })
            }
        }
    }

    @action loadUserWatchedInfoForSelectedSeries = async () => {
        if(this.selectedSeries) {
            var watchedInfo = await agent.watching.getSeriesWatchedInfo(this.selectedSeries.seriesId);
            if(watchedInfo) {
                runInAction("loading selected series watching info", () => {
                    this.selectedSeriesWatchedInfo = watchedInfo;
                })
            }
        }
    }

    @action reviewSelectedSeries = async (review: ISeriesReviewRequest) => {
        if(this.selectedSeries) {
            var response = await agent.review.reviewSeries(this.selectedSeries.seriesId, review);
            runInAction("updating user review", () => {
                if(response) {
                    this.selectedSeriesUserReview = response;
                }
            })
        }
    };

    @action rateSelectedSeries = async (review: ISeriesRateRequest) => {
        if(this.selectedSeries) {
            var response = await agent.review.rateSeries(this.selectedSeries.seriesId, review);
            runInAction("updating user review", () => {
                if(response) {
                    this.selectedSeriesUserReview = response;
                }
            })
        }
    }
    
    @action watchSelectedSeries = async () => {
          if(this.selectedSeries) {
            let response = await agent.watching.watchSeries(this.selectedSeries.seriesId);
            runInAction(() => {
                if (response && this.selectedSeriesWatchedInfo) {
                  this.selectedSeriesWatchedInfo.isWatchingSeries = true;
                }
              });
          }
    };

    @action likeSelectedSeries = async () => {
        if(this.selectedSeries) {
          let response = await agent.watching.likeSeries(this.selectedSeries.seriesId);
          runInAction(() => {
              if (response && this.selectedSeriesWatchedInfo) {
                this.selectedSeriesWatchedInfo.hasLikedSeries = true;
              }
            });
        }
  };
    
    @action watchEpisodeOfSelectedSeries = async (episodeId: number) => {
        if(this.selectedSeries) {
            console.log(this.selectedSeriesWatchedInfo);
            await agent.watching.watchEpisode(episodeId);
            if(this.selectedSeriesWatchedInfo.episodesWatchedIdList) {
                this.selectedSeriesWatchedInfo.episodesWatchedIdList.push(episodeId);
                this.selectedSeriesWatchedInfo.isWatchingSeries = true;
                console.log(this.selectedSeriesWatchedInfo);    
            }
        }
    }

    @action loadReviewListForSelectedSeries = async () => {
        if(this.selectedSeries) {
            this.selectedSeries.reviews = [];
            var reviewList = await agent.review.getSeriesReviews(this.selectedSeries.seriesId);
            if(reviewList) {
                runInAction("loading selected series reviews", () => {
                    if(this.selectedSeries) {
                        for(const review of reviewList) {
                            this.selectedSeries.reviews.push(review);
                        }
                    }
                })
            }
        }
    }

    @action loadCategories = async () => {
        try {
            const categoryList = await agent.categories.list();
            runInAction("loading series", () => {
                for (const category of categoryList) {
                    this.categoryRegistry.set(category.categoryId, category);
                }
            });
        } catch (error) {
            console.log(error);
        }
    }

    @action loadSeasonEpisodes = async (seriesId: number, seasonNumber: number) => {
        try {
            this.clearLoadedEpisodes();
            var episodes = await agent.series.getSeason(seriesId, seasonNumber);
            runInAction("loading season", () => {
                for (const episode of episodes) {
                    this.loadedEpisodes.push(episode);
                }
            })
        } catch (error) {
            console.log(error);
        }
    }

    @computed get categoriesForDropdown(): DropdownItemProps[] {
        return Array.from(this.categoryRegistry.values()).map(c => ({
          key: c.categoryId,
          text: c.categoryName,
          value: c.categoryId
        }));
    }

    @computed get seasonsForDropdown(): DropdownItemProps[] {
        if(this.selectedSeries) {
            return this.selectedSeries.seasonNumbers.sort().map(s => ({
                key: s,
                text: String(s),
                value: s
            }));
        } else {
            return [];
        }

    }

}

export default SeriesStore;

