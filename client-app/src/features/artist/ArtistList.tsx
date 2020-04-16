import { observer } from "mobx-react-lite";
import React, { useState, useContext, useEffect, SyntheticEvent, FormEvent } from 'react'
import { IArtistParams, ArtistOccupation, ArtistSortBy } from "../../app/models/searchParams";
import SeriesStore from "../../app/stores/seriesStore";
import { Segment, Form, Grid, Pagination, DropdownItemProps, DropdownProps } from "semantic-ui-react";
import ArtistCard from "./ArtistCard";
import ArtistStore from "../../app/stores/artistStore";


const ArtistList = () => {
    const seriesStore = useContext(ArtistStore);

    const { artistRegistry, loadArtists, currentPage, totalPages, setCurrentPage} = seriesStore;

    const [searchParams, setsearchParams] = useState<IArtistParams>({
        nameFilter: "",
        occupation: ArtistOccupation.Any,
        sort: ArtistSortBy.Alphabetical
    });

    useEffect(() => {
        loadArtists(getAxiosParams());
    }, [currentPage])

    const getAxiosParams = (): URLSearchParams => {
        var params = new URLSearchParams();
        if(searchParams.nameFilter != "") {
            params.append("NameFilter", searchParams.nameFilter);
        }
        params.append("Occupation", searchParams.occupation.toString());
        params.append("PageNumber", currentPage.toString());
        params.append("Sort", searchParams.sort.toString());
        return params;
    }

    const getSortByOptions = () : DropdownItemProps[] => {
        return  [
            {
                key: ArtistSortBy.Alphabetical,
                text: "Alphabetical",
                value: ArtistSortBy.Alphabetical
            },
            {
                key: ArtistSortBy.AlphabeticalDescending,
                text: "Alphabetical reverse",
                value: ArtistSortBy.AlphabeticalDescending
            }
        ];
    }

    const handleOccupationChange = (event: any, data: DropdownProps) => {
        if (data.value) {
            setsearchParams({ ...searchParams, ["occupation"]: Number(data.value) });
            console.log(searchParams);
        }
      };
    
    const handleSortByChange = (event: any, data: DropdownProps) => {
        if (data.value) {
            setsearchParams({ ...searchParams, ["sort"]: Number(data.value) });
        }
      };

    const getOccupationOptions = () : DropdownItemProps[] => {
        return  [
            {
                key: ArtistOccupation.Actor,
                text: "Actor",
                value: ArtistOccupation.Actor
            },
            {
                key: ArtistOccupation.Writer,
                text: "Writer",
                value: ArtistOccupation.Writer
            },
            {
                key: ArtistOccupation.Both,
                text: "Actor and Writer",
                value: ArtistOccupation.Both
            },
            {
                key: ArtistOccupation.Any,
                text: "Any",
                value: ArtistOccupation.Any
            }
        ];
    }

    const handlePageChange = (
        event: SyntheticEvent, data: any
        ) => {
            setCurrentPage(data.activePage);
        }
    
    const handleFilterChange = (
        event: FormEvent<HTMLInputElement>
        ) => {
            const { name, value } = event.currentTarget;
            setsearchParams({ ...searchParams, [name]: value });
            console.log(searchParams);
          };



    return(
        <Segment clearing style={{marginTop: '2em' }}>

            <Form style={{marginLeft: '2em', marginRight: '2em'}}>
                <Form.Group>
                    <Form.Dropdown 
                        selection
                        width={4}
                        title="Occupation"
                        name="occupation"
                        placeholder="Occupation" 
                        options={getOccupationOptions()}
                        value={searchParams.occupation}
                        onChange={handleOccupationChange} 
                    />

                    <Form.Dropdown
                        width={4}
                        title="Sort by"
                        placeholder="Sort by"
                        selection
                        options={getSortByOptions()}
                        value={searchParams.sort}
                        onChange={handleSortByChange}
                    />

                    <Form.Input
                        width={8} 
                        onChange={handleFilterChange} 
                        name="nameFilter" placeholder="Artist Name"
                        icon="search"
                        value={searchParams.nameFilter}
                    />

                    <Form.Button primary fluid onClick={() => {
                        setCurrentPage(1);
                        loadArtists(getAxiosParams());
                    }}>
                        Search
                    </Form.Button>

                </Form.Group>
            </Form>

            <Grid columns={5} container doubling stackable>
                {Array.from(artistRegistry.values()).map(artist => (
                    <Grid.Column>
                        <ArtistCard artist={artist} />
                    </Grid.Column>
                ))}
            </Grid>


            <Pagination style={{marginTop: '1em', marginLeft: '2em', align:'center'}}
                boundaryRange={0}
                defaultActivePage={1}
                activePage={currentPage}
                ellipsisItem={null}
                firstItem={null}
                lastItem={null}
                siblingRange={3}
                totalPages={totalPages}
                onPageChange={handlePageChange}
            />
        </Segment>
    )
}


export default observer(ArtistList);