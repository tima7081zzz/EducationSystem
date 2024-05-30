import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import HomePage from './pages/HomePage';
import CourseDetailsPage from './pages/CourseDetailsPage';
import AssignmentPage from './pages/AssignmentDetailsPage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import Layout from './components/common/Layout';
import AssignmentOverviewPage from './pages/AssignmentOverviewPage';
import SettingsPage from './pages/SettingsPage';

const App: React.FC = () => {
  return (
    <Router>
      <Layout children={undefined} />
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/home" element={<HomePage />} />
        <Route path="/course/:courseId" element={<CourseDetailsPage />} />
        <Route path="/course/:courseId/assignment/:assignmentId" element={<AssignmentPage />} />
        <Route path="/course/:courseId/assignment/:assignmentId/overview" element={<AssignmentOverviewPage/>} />
        <Route path="/" element={<Navigate to="/home" />} />
        <Route path="/settings" element={<SettingsPage />} />
      </Routes>
    </Router>
  );
};

export default App;
