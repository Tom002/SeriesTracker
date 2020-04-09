import {observable, action, runInAction, computed} from 'mobx';
import { ISeries, ICategory } from '../models/series';
import agent from '../api/agent';
import { createContext } from 'react';
import { DropdownItemProps } from 'semantic-ui-react';
import { ISeriesParams, IPagination } from '../models/searchParams';
import { IArtist } from '../models/artist';

class SeriesStore {
    @observable seriesRegistry = new Map<number, ISeries>();
    @observable categoryRegistry = new Map<number, ICategory>();
    

    @observable currentPage = 1;
    @observable totalPages = 1;

    @action setCurrentPage = (page: number) => {
        runInAction("setting page", () => {
            console.log("page param:" + page);
            this.currentPage = page;
            console.log(this.currentPage);
        })
    }
    
    @action setTotalPages = (page: number) => {
        runInAction("setting total pages", () => {
            this.totalPages = page;
        })
    }

   
  
    @action loadSeries = async (params: URLSearchParams) => {
        this.seriesRegistry.clear();
        try {
            const response = await agent.series.list(params);
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

    @action loadCategories = async () => {
        try {
            const categoryList = await agent.categories.list();
            console.log(categoryList);
            runInAction("loading series", () => {
                for (const category of categoryList) {
                    this.categoryRegistry.set(category.categoryId, category);
                }
            });
        } catch (error) {
            
        }
    }

    @computed get categoriesForDropdown(): DropdownItemProps[] {
        return Array.from(this.categoryRegistry.values()).map(c => ({
          key: c.categoryId,
          text: c.categoryName,
          value: c.categoryId
        }));
    }

}

export default createContext(new SeriesStore());

