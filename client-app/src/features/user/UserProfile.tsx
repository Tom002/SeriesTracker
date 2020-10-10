import { observer } from "mobx-react-lite";
import React, { useContext, useEffect } from "react";
import { Fragment } from "react";
import { RouteComponentProps } from "react-router-dom";
import { Grid, Tab, Image, Header } from "semantic-ui-react";
import RootStore from "../../app/stores/rootStore";
import { AboutUser } from "./AboutUser";
import { SeriesLikedTab } from "./SeriesLikedTab";
import { SeriesReviewedTab } from "./SeriesReviewedTab";
import { SeriesWatchedTab } from "./SeriesWatchedTab";

interface UserProfileParams  {
    id: string;
}

const UserProfile : React.FC<RouteComponentProps<UserProfileParams>> = ({match}) => {

    const rootStore = useContext(RootStore);

    useEffect(() => {
        if(match.params.id){
            rootStore.profileStore.loadProfile(match.params.id);
        }

        return () => {
            rootStore.profileStore.clearProfileData();
        }
    }, [])


    const panes = [
        { menuItem: 'Profile', render: () =>
            <Tab.Pane>
                <AboutUser profile={rootStore.profileStore.loadedProfile} />
            </Tab.Pane> 
        },
        { menuItem: 'Series watched', render: () => 
            <Tab.Pane>
                <SeriesWatchedTab seriesList={rootStore.profileStore.watchedSeries} />
            </Tab.Pane> 
        },
        { menuItem: 'Series liked', render: () => 
            <Tab.Pane>
                <SeriesLikedTab seriesList={rootStore.profileStore.likedSeries} />
            </Tab.Pane> 
        },
        { menuItem: 'Reviews', render: () => 
            <Tab.Pane>
                <SeriesReviewedTab reviewList={rootStore.profileStore.reviews} />
            </Tab.Pane> 
        },
      ]

    return(
        <Fragment>
            <Grid style={{ marginLeft: "2em", marginTop: "2em", marginRight: "2em" }}>
                <Grid.Column width={4} floated="left">
                    <Image src={rootStore.profileStore.loadedProfile?.profileImageUrl 
                                    ? rootStore.profileStore.loadedProfile.profileImageUrl
                                    : "http://via.placeholder.com/400" 
                                } size="medium" />
                    <Header size='huge' textAlign='center'>
                        {rootStore.profileStore.loadedProfile?.name}
                    </Header>
                </Grid.Column>
                <Grid.Column width={12}>
                    <Tab panes={panes} />
                </Grid.Column>
            </Grid>
        </Fragment>
    )
    

}
export default observer(UserProfile);