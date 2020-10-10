import React, {useContext, useEffect, useState, FormEvent, SyntheticEvent, Fragment} from 'react'
import { Grid, Image, Segment, Form, Dropdown, DropdownProps, DropdownItemProps, DropdownItem, Pagination } from 'semantic-ui-react';
import SeriesCard from './SeriesCard';
import { observer } from "mobx-react-lite";
import { ISeriesParams, SeriesSortBy } from '../../app/models/searchParams';
import RootStore from '../../app/stores/rootStore';


const SeriesList = () => {
    const rootStore = useContext(RootStore);

    const {
        loadSeries,
        loadCategories,
        seriesRegistry,
        categoriesForDropdown,
        totalPages
    } = rootStore.seriesStore;

    const [searchParams, setsearchParams] = useState<ISeriesParams>({
        sortBy :SeriesSortBy.Alphabetical,
        categoryId : 0,
        titleFilter : "",
    });

    const [page, setPage] = useState(1);

    const getAxiosParams = () : URLSearchParams => {
        const params = new URLSearchParams();
        if(searchParams.categoryId != 0) {
            params.append('CategoryId', searchParams.categoryId.toString());
        }
        if(searchParams.titleFilter) {
            params.append('TitleFilter', searchParams.titleFilter);
        }
        console.log(page);
        params.append('PageNumber', page.toString());
        params.append('Sort', searchParams.sortBy.toString());
        return params;
      }

    const handleCategoryChange = (event: any, data: DropdownProps) => {
        if (data.value) {
            console.log(event);
            setsearchParams({ ...searchParams, ["categoryId"]: Number(data.value) });
            console.log(searchParams);
        }
      };
    
    const handleSortByChange = (event: any, data: DropdownProps) => {
        if (data.value) {
            setsearchParams({ ...searchParams, ["sortBy"]: Number(data.value) });
            console.log(searchParams);
        }
      };

    const getSortByOptions = () : DropdownItemProps[] => {
        return  [
            {
                key: SeriesSortBy.HighestRating,
                text: "Highest rating",
                value: SeriesSortBy.HighestRating
            },
            {
                key: SeriesSortBy.LowestRating,
                text: "Lowest rating",
                value: SeriesSortBy.LowestRating
            },
            {
                key: SeriesSortBy.MostReviewed,
                text: "Most reviewed",
                value: SeriesSortBy.MostReviewed
            },
            {
                key: SeriesSortBy.Alphabetical,
                text: "Alphabetical",
                value: SeriesSortBy.Alphabetical
            },
            {
                key: SeriesSortBy.AlphabeticalDescending,
                text: "Alphabetical reverse",
                value: SeriesSortBy.AlphabeticalDescending
            }
        ];
    }
    
      const handleFilterChange = (
        event: FormEvent<HTMLInputElement>
      ) => {
        const { name, value } = event.currentTarget;
        setsearchParams({ ...searchParams, [name]: value });
        console.log(searchParams);
      };

      const handlePageChange = (
          event: SyntheticEvent, data: any
          ) => {
              setPage(data.activePage);
          }

    useEffect(() => {   
        loadSeries(getAxiosParams());
        loadCategories();
    }, [page])

    return(
        <Segment clearing style={{marginTop: '2em' }}>
            <Form style={{marginLeft: '2em', marginRight: '2em'}}>
                <Form.Group>
                    <Form.Dropdown 
                        selection
                        width={4}
                        title="Category"
                        name="categoryId"
                        placeholder="Category" 
                        options={categoriesForDropdown}
                        onChange={handleCategoryChange} 
                    />

                    <Form.Dropdown
                        width={4}
                        title="Sort by"
                        placeholder="Sort by"
                        selection
                        options={getSortByOptions()}
                        value={searchParams.sortBy}
                        onChange={handleSortByChange}
                    />

                    <Form.Input
                        width={8} 
                        onChange={handleFilterChange} 
                        name="titleFilter" placeholder="Series title"
                        icon="search"
                        value={searchParams.titleFilter} 
                    />

                    <Form.Button primary fluid onClick={() => {
                        setPage(1);
                        loadSeries(getAxiosParams())
                    }}>
                        Search
                    </Form.Button>

                </Form.Group>
            </Form>

            <Grid style={{marginLeft: '2em', marginRight: '2em'}}>
                <Grid.Row columns={5} stretched>
                {Array.from(seriesRegistry.values()).map(series => (
                    <Grid.Column>
                        <SeriesCard series={series} />
                    </Grid.Column>
                ))}
                </Grid.Row>
            </Grid>

            <Pagination style={{marginTop: '1em', marginLeft: '2em', align:'center'}}
                boundaryRange={0}
                defaultActivePage={1}
                activePage={page}
                ellipsisItem={null}
                firstItem={null}
                lastItem={null}
                siblingRange={1}
                totalPages={totalPages}
                onPageChange={handlePageChange}
            />
        </Segment>

    );
}

export default observer(SeriesList);