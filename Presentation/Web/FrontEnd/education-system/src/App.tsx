import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import HomePage from './pages/HomePage';
import CourseDetailsPage from './pages/CourseDetailsPage';
import AssignmentDetailsPage from './pages/AssignmentDetailsPage';
import Layout from './components/Layout';

const App: React.FC = () => {
  return (
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/course/:id" element={<CourseDetailsPage />} />
          <Route path="/course/:courseId/assignment/:assignmentId" element={<AssignmentDetailsPage />} />
        </Routes>
      </Layout>
    </Router>
  );
};

export default App;
