import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './pages/Home';
import Upload from './pages/Upload';
import { QueryParamProvider } from 'use-query-params';



const App = () => {
    return (
        <QueryParamProvider ReactRouterRoute={Route}>
            <Layout>
                <Route exact path='/' component={Home} />
                <Route exact path='/Upload' component={Upload} />
            </Layout>
        </QueryParamProvider>

    );
}

export default App;