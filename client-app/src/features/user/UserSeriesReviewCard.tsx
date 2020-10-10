import React from "react";
import { Link } from "react-router-dom";
import { Button, Card, Comment, Header, Icon, Segment } from 'semantic-ui-react'
import { IReview, ISeriesWatchedListItem, IUserSeriesReview, IUserSeriesReviewInfo } from "../../app/models/series";

const UserSeriesReviewCard: React.FC<{review: IUserSeriesReview}> = (
    {review
}) => {
    return (
        <Card style={{marginTop: '2em' }}>
            <Card.Content header={review.reviewTitle} />
            <Card.Description style={{margin: '2em'}}>
                {review.reviewText}
            </Card.Description>
            <Card.Content extra>
                <Segment  textAlign='center'>
                    Rating: <Icon name='star' /> {review.rating}
                </Segment>
                <Segment textAlign='center'>
                    <Button primary as={Link} size='small'
                                to={`/series/${review.seriesId}`}
                                content={review.seriesTitle} />
                </Segment>
            </Card.Content>
        </Card>
    )
}
export default UserSeriesReviewCard;