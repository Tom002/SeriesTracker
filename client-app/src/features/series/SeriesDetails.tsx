import { ISeriesForList } from "../../app/models/series";
import React, { useContext, useEffect, Fragment, useState } from 'react';
import { RouteComponentProps } from "react-router-dom";
import SeriesStore from '../../app/stores/seriesStore';
import UserStore from '../../app/stores/userStore';
import { observer } from "mobx-react-lite";
import { Grid,Image, Divider, Segment, Header, List, LabelGroup, Label, Statistic, Rating, Icon, Table, Button, Card, Dropdown, DropdownItemProps, Item, DropdownProps, Container } from "semantic-ui-react";
import moment from "moment";

interface SeriesDetailParams {
    id: string;
}

const SeriesDetails: React.FC<RouteComponentProps<SeriesDetailParams>> = ({
    match,
    history
}) =>{
    const seriesStore = useContext(SeriesStore);
    const userStore = useContext(UserStore);
    const {getSingleSeries, selectedSeries, seasonsForDropdown, loadedEpisodes, loadSeasonEpisodes, clearLoadedEpisodes} = seriesStore;
    const {currentUser, userProfile, seriesWatched, getSeriesWatched, watchSeries, episodesWatched, watchEpisode, getEpisodesWatchedForSeries} = userStore;
    const [selectedSeason, setselectedSeason] = useState(1);

    useEffect(() => {
        clearLoadedEpisodes();
        if(match.params.id) {
            let id = Number(match.params.id);
            if(isNaN(id)) {
            }
            else {
                getSingleSeries(id).then(() => {
                    if(currentUser && !currentUser.expired) {
                        getSeriesWatched(currentUser.profile.sub);
                        if(selectedSeries) {
                            getEpisodesWatchedForSeries(currentUser.profile.sub, selectedSeries.seriesId);
                        }  
                    }
                })

            }
        }
    }, [])

    const handleSeasonChange = (event: any, data: DropdownProps) => {
        if (data.value) {
            setselectedSeason(Number(data.value));
        }
      };
    
    const loadSeason = () => {
        if(selectedSeries && selectedSeason) {
            loadSeasonEpisodes(selectedSeries.seriesId, selectedSeason);
        }
    }

    return(
        <Fragment >
            <Grid style={{marginLeft: '2em', marginTop: '2em', marginRight: '2em'}}>
                <Grid.Column width={4} floated="left">
                        <Image src={selectedSeries?.coverImageUrl} size='medium' />
                </Grid.Column>
                <Grid.Column width={8}>
                    <Header textAlign="center" size='huge'>{selectedSeries?.title}</Header>
                    <LabelGroup color='blue'>
                    {selectedSeries?.categories.map(category  => (
                        <Label as='a'>
                            {category.categoryName}
                        </Label>
                    ))}
                    </LabelGroup>
                    <Container>
                        {selectedSeries?.description}
                    </Container>
                    <Header size='medium'>
                        Writers: {selectedSeries?.writers.map(writer => (
                            <Label as='a'>
                                {writer.name}
                            </Label>
                    ))}
                    </Header>

                    <Header size='medium'>
                        Stars: {selectedSeries?.cast.sort(c => c.order).slice(0,3).map(cast => (
                            <Label as='a'>
                                {cast.name}
                            </Label>
                    ))}
                    </Header>
                       
                </Grid.Column>
                <Grid.Column width={4}>
                        <Statistic floated='right' size='tiny'>
                        <Statistic.Value>{selectedSeries?.ratingAverage}/5 <Rating rating={1} size='huge' disabled /></Statistic.Value>
                            <Statistic.Label>Average</Statistic.Label>
                        </Statistic>

                        <Statistic floated='left' size='tiny'>
                            <Statistic.Value>{selectedSeries?.ratingsCount}</Statistic.Value>
                            <Statistic.Label>Ratings</Statistic.Label>
                        </Statistic>
                    {currentUser && userProfile && !currentUser.expired && (
                        <Table celled>
                        <Table.Body>
                            <Table.Row>
                                <Table.Cell>
                                    {selectedSeries && seriesWatched.includes(selectedSeries.seriesId) 
                                        ?     
                                            <Button 
                                            content='Watched' 
                                            icon='eye slash outline'
                                            size='mini'
                                            />  
                                        : selectedSeries && 
                                          <Button 
                                            content='Watch' 
                                            icon='eye large'
                                            size='mini'
                                            onClick={() => watchSeries(currentUser.profile.sub, selectedSeries.seriesId)}
                                          />  
                                    }
                                </Table.Cell>
                                <Table.Cell textAlign='center'>
                                    <Button size='mini' content='Like' icon='heart outline' />
                                </Table.Cell>
                                <Table.Cell textAlign='center'>
                                    <Button size='mini' content='Follow' icon='calendar plus outline' />
                                </Table.Cell>
                            </Table.Row>
                            <Table.Row textAlign='center'>
                                <Table.Cell colspan='3'>
                                    <Header size='medium'>Rate Series</Header>
                                    <Rating maxRating={5} size='large' clearable />
                                </Table.Cell>
                            </Table.Row>
                            <Table.Row textAlign='center'>
                                <Table.Cell colspan='3'>
                                    <Button>
                                        Add a review
                                    </Button>
                                </Table.Cell>
                            </Table.Row>
                        </Table.Body>
                    </Table>
                    )}
                </Grid.Column>
            </Grid>
            
            <Grid floated='left' style={{marginLeft: '2em', marginTop: '2em', marginRight: '2em'}}>
                <Grid.Column width={6}>
                        <Header textAlign="center" size='medium'>Cast</Header>
                    <Grid doubling columns={3}>
                            {selectedSeries?.cast.map(actor => (
                                <Grid.Column>
                                    <Card textAlign='center'>
                                        <Image size='small' src={actor.imageUrl}/>
                                        <Card.Content>
                                            <Card.Header>{actor.name}</Card.Header>
                                            <Card.Meta>as {actor.roleName}</Card.Meta>
                                        </Card.Content>
                                    </Card>
                                </Grid.Column>
                            ))}
                    </Grid>
                </Grid.Column>
                <Grid.Column width={10}>
                    <Container textAlign='center'>
                        <Header size='medium'>Episodes</Header>
                        <Dropdown 
                            value={selectedSeason}
                            onChange={handleSeasonChange}  
                            placeholder='Season number' 
                            selection 
                            options={seasonsForDropdown}
                        />
                        <Button style={{marginLeft: '1em'}} onClick={loadSeason}> 
                            List episodes
                        </Button>
                    </Container>

                    <Item.Group divided>
                        {
                            loadedEpisodes
                            .filter(e => e.season === selectedSeason)
                            .map(episode => (
                                <Item key={episode.episodeId}>
                                    <Item.Image src={episode.coverImageUrl} />
                                    <Item.Content>
                                        <Item.Header as='a'>{episode.episodeTitle}</Item.Header>
                                        <Item.Meta>
                                            { 
                                            episode.release ? 
                                                moment(episode.release).format('YYYY/MM/DD') 
                                                : 'Release unknown'   
                                            }
                                        </Item.Meta>
                                        <Item.Description>{episode.description}</Item.Description>
                                        <Item.Extra>
                                            <Label>Season {episode.season}</Label>
                                            <Label>Episode {episode.episodeNumber}</Label>
                                            {currentUser && userProfile && !currentUser.expired && selectedSeries && (
                                                episodesWatched.includes(episode.episodeId) 
                                                    ?
                                                        <Button floated='right'>
                                                            Watched
                                                            <Icon style={{marginLeft:'0.5em'}}  name='eye slash outline' />
                                                        </Button>
                                                    :
                                                        <Button floated='right' 
                                                            onClick = {() => 
                                                            {
                                                                watchEpisode(currentUser?.profile.sub,
                                                                    selectedSeries.seriesId,
                                                                    episode.episodeId,
                                                                    new Date(),
                                                                    false
                                                                    ).then(() => {
                                                                        console.log("episodes watch after new watch");
                                                                        clearLoadedEpisodes();
                                                                        loadSeason();
                                                                    })
                                                            }}
                                                        >
                                                            Watch
                                                            <Icon style={{marginLeft:'0.5em'}}  name='eye' />
                                                        </Button>
                                            )}
                                        </Item.Extra>
                                    </Item.Content>
                                </Item>
                            ))
                        }
                    </Item.Group>
                </Grid.Column>
            </Grid>
        </Fragment>
        
    )
}



export default observer(SeriesDetails);