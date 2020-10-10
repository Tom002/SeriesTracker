import React, { useContext, useEffect, Fragment, useState, SyntheticEvent, FormEvent } from "react";
import { Comment, Form, Input, TextArea } from 'semantic-ui-react'
import { RouteComponentProps } from "react-router-dom";
import SeriesStore from "../../app/stores/seriesStore";
import UserStore from "../../app/stores/userStore";
import { observer } from "mobx-react-lite";
import {
  Grid,
  Image,
  Divider,
  Segment,
  Header,
  List,
  LabelGroup,
  Label,
  Statistic,
  Rating,
  Icon,
  Table,
  Button,
  Card,
  Dropdown,
  DropdownItemProps,
  Item,
  DropdownProps,
  Container,
  RatingProps,
} from "semantic-ui-react";
import moment from "moment";
import { ISeriesRateRequest, ISeriesReviewRequest } from "../../app/models/series";
import agent from "../../app/api/agent";
import { wait } from "@testing-library/react";
import RootStore from "../../app/stores/rootStore";
import SeriesReview from "./SeriesReview";
import MyReview from "./MyReview";

interface SeriesDetailParams {
  id: string;
}

const SeriesDetails: React.FC<RouteComponentProps<SeriesDetailParams>> = ({
  match,
  history,
}) => {
  const rootStore = useContext(RootStore);

  const [newSeriesReview, setNewSeriesReview] = useState<ISeriesReviewRequest>(
    {
      reviewTitle: "",
      reviewText: ""
    }
  )

  const handleInputChange = (
    event: FormEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = event.currentTarget;
    setNewSeriesReview({ ...newSeriesReview, [name]: value });
  };

  const {
    clearLoadedEpisodes,
    clearSelectedSeries,
    getSingleSeries,
    rateSelectedSeries,
    reviewSelectedSeries,
    watchSelectedSeries,
    watchEpisodeOfSelectedSeries,
    loadSeasonEpisodes,
    seasonsForDropdown,
    selectedSeries,
    selectedSeriesUserReview,
    selectedSeriesWatchedInfo,
    loadedEpisodes,
    likeSelectedSeries
  } = rootStore.seriesStore;
  
  const {
    currentUser,
    userProfile,
  } = rootStore.userStore;
  
  const [selectedSeason, setselectedSeason] = useState(1);

  useEffect(() => {
    if (match.params.id) {
        let id = Number(match.params.id);
        getSingleSeries(id);
    }
    return () => {
      clearLoadedEpisodes();
      clearSelectedSeries();
    }  
  }
  , []);

  const handleSeasonChange = (event: any, data: DropdownProps) => {
    if (data.value) {
      setselectedSeason(Number(data.value));
    }
  };

  const loadSeason = () => {
    if (selectedSeries && selectedSeason) {
      loadSeasonEpisodes(selectedSeries.seriesId, selectedSeason);
    }
  };

  const addSeriesRating = async (event: SyntheticEvent, data: RatingProps) => {
    if(currentUser && !currentUser.expired && data.rating) {
      var reviewRequest: ISeriesRateRequest = {
        rating: Number(data.rating)
      };
      rateSelectedSeries(reviewRequest).then(() => {
      })
      .catch(() => {
        console.log("Review failed");
      })
    }
  }

  const addSeriesReview = async () => {
    if(currentUser && 
      !currentUser.expired &&
      selectedSeries &&
      newSeriesReview.reviewText.length > 0 &&
      newSeriesReview.reviewTitle.length > 0) {
        await reviewSelectedSeries(newSeriesReview);
      }
  }

  return (
    <Fragment>
      <Grid style={{ marginLeft: "2em", marginTop: "2em", marginRight: "2em" }}>
        <Grid.Column width={4} floated="left">
          <Image src={selectedSeries?.coverImageUrl} size="medium" />
        </Grid.Column>
        <Grid.Column width={8}>
          <Header textAlign="center" size="huge">
            {selectedSeries?.title}
          </Header>
          <LabelGroup color="blue">
            {selectedSeries?.categories.map((category) => (
              <Label as="a">{category.categoryName}</Label>
            ))}
          </LabelGroup>
          <Container>{selectedSeries?.description}</Container>
          <Header size="medium">
            Writers:{" "}
            {selectedSeries?.writers.map((writer) => (
              <Label as="a">{writer.name}</Label>
            ))}
          </Header>

          <Header size="medium">
            Stars:{" "}
            {selectedSeries?.cast
              .sort((c) => c.order)
              .slice(0, 3)
              .map((cast) => (
                <Label as="a">{cast.name}</Label>
              ))}
          </Header>
        </Grid.Column>
        <Grid.Column width={4}>
          <Statistic floated="right" size="tiny">
            <Statistic.Value>
              {selectedSeries?.ratingAverage}/5{" "}
              <Rating rating={1} size="huge" disabled />
            </Statistic.Value>
            <Statistic.Label>Average</Statistic.Label>
          </Statistic>

          <Statistic floated="left" size="tiny">
            <Statistic.Value>{selectedSeries?.ratingsCount}</Statistic.Value>
            <Statistic.Label>Ratings</Statistic.Label>
          </Statistic>
          {currentUser && userProfile && !currentUser.expired && (
            <Table celled>
              <Table.Body>
                <Table.Row>
                  <Table.Cell>
                    {(selectedSeriesWatchedInfo?.isWatchingSeries 
                      ?  <Button content="Watched" size="mini" />
                      :  <Button
                            content="Watch"
                            size="mini"
                            onClick={() => watchSelectedSeries()}/>
                    )}
                  </Table.Cell>
                  <Table.Cell textAlign="center">
                  {(selectedSeriesWatchedInfo?.hasLikedSeries 
                      ?  <Button content="Liked" size="mini" />
                      :  <Button
                            content="Like"
                            size="mini"
                            onClick={() => likeSelectedSeries()}/>
                    )}
                  </Table.Cell>
                </Table.Row>
                <Table.Row textAlign="center">
                  <Table.Cell colspan="2">
                    {(currentUser && selectedSeriesUserReview?.isReviewedByUser && selectedSeriesUserReview.rating 
                      ?  <Header size="medium">Your Rating</Header>
                      :  <Header size="medium">Rate Series</Header>
                    )}
                    <Rating
                      onRate={addSeriesRating}
                      maxRating={5}
                      rating={(selectedSeriesUserReview && selectedSeriesUserReview.rating) 
                                ? selectedSeriesUserReview.rating 
                                : undefined}
                      size="large"
                    />
                  </Table.Cell>
                </Table.Row>
                <Table.Row textAlign="center">
                  <Table.Cell colspan="2">
                    { }
                    <Button>Add a review</Button>
                  </Table.Cell>
                </Table.Row>
              </Table.Body>
            </Table>
          )}
        </Grid.Column>
      </Grid>
      
      <Segment vertical>                
        <Grid
          floated="left"
          style={{ marginLeft: "2em", marginTop: "2em", marginRight: "2em" }}
        >
          <Grid.Column width={6}>
            <Header textAlign="center" size="medium">
              Cast
            </Header>
            
            <Grid doubling columns={3}>
              {selectedSeries?.cast.map((actor) => (
                <Grid.Column>
                  <Card textAlign="center">
                    <Image size="small" src={actor.imageUrl} />
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
            <Container textAlign="center">
              <Header size="medium">Episodes</Header>
              <Dropdown
                value={selectedSeason}
                onChange={handleSeasonChange}
                placeholder="Season number"
                selection
                options={seasonsForDropdown}
              />
              <Button style={{ marginLeft: "1em" }} onClick={loadSeason}>
                List episodes
              </Button>
            </Container>

            <Item.Group divided>
              {loadedEpisodes
                .filter((e) => e.season === selectedSeason)
                .map((episode) => (
                  <Item key={episode.episodeId}>
                    <Item.Image src={episode.coverImageUrl} />
                    <Item.Content>
                      <Item.Header as="a">{episode.episodeTitle}</Item.Header>
                      <Item.Meta>
                        {episode.release
                          ? moment(episode.release).format("YYYY/MM/DD")
                          : "Release unknown"}
                      </Item.Meta>
                      <Item.Description>{episode.description}</Item.Description>
                      <Item.Extra>
                        <Label>Season {episode.season}</Label>
                        <Label>Episode {episode.episodeNumber}</Label>
                        {currentUser && userProfile && !currentUser.expired &&
                          (
                            selectedSeries && selectedSeriesWatchedInfo?.episodesWatchedIdList &&
                              selectedSeriesWatchedInfo.episodesWatchedIdList.includes(episode.episodeId) 
                              ? (
                                <Button floated="right">
                                  Watched
                                  <Icon
                                    style={{ marginLeft: "0.5em" }}
                                    name="eye slash outline"
                                  />
                                </Button>
                              )
                              : (
                                <Button
                                  floated="right"
                                  onClick={() => watchEpisodeOfSelectedSeries(episode.episodeId)}
                                >
                                  Watch
                                  <Icon style={{ marginLeft: "0.5em" }} name="eye" />
                                </Button>
                                ) 
                          )
                        }
                      </Item.Extra>
                    </Item.Content>
                  </Item>
                ))}
            </Item.Group>
          </Grid.Column>
        </Grid>
      </Segment>  
      
      <Segment vertical secondary>
        <Grid
          floated="left"
          style={{ marginLeft: "2em", marginTop: "2em", marginRight: "2em" }}
        >
          <Grid.Column width={12}>
              <Header textAlign="center" size="medium">
                Reviews
              </Header>
              <Card.Group>
                {selectedSeries?.reviews && selectedSeries.reviews.map(review => (
                  <SeriesReview review={review} />
                ))} 
              </Card.Group>       
          </Grid.Column>

          <Grid.Column width={4}>
            {selectedSeriesUserReview?.isReviewedByUser && 
            selectedSeriesUserReview.reviewText &&
            selectedSeriesUserReview.reviewTitle
            ? (
              <Fragment>
                <Header textAlign="center" size="medium">
                  Your review
                </Header>
                <MyReview review={selectedSeriesUserReview} />
              </Fragment>
              )
            : (
              <Fragment>
                  <Header textAlign="center">
                    Add a review
                  </Header>
                  <Form onSubmit={addSeriesReview}>
                    <Form.Input 
                      required 
                      onChange={handleInputChange}
                      name='reviewTitle' 
                      placeholder='Review Title' 
                      value={newSeriesReview.reviewTitle} 
                    />
                    <Form.Input 
                      required 
                      onChange={handleInputChange}
                      name='reviewText' 
                      placeholder='Review Text' 
                      value={newSeriesReview.reviewText} 
                    />
                    <Form.Button content='Add Review' />
                  </Form>
              </Fragment>
              )
            } 
          </Grid.Column>
        </Grid>
      </Segment>
      
    </Fragment>
  );
};

export default observer(SeriesDetails);
