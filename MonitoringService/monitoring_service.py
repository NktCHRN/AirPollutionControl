from flask import Flask, jsonify, request
from flask_cors import CORS
import openmeteo_requests
import requests_cache
import pandas as pd
from retry_requests import retry
from geopy.geocoders import Nominatim

app = Flask(__name__)
CORS(app)

# Setup the Open-Meteo API client with cache and retry on error
cache_session = requests_cache.CachedSession('.cache', expire_after=3600)
retry_session = retry(cache_session, retries=5, backoff_factor=0.2)
openmeteo = openmeteo_requests.Client(session=retry_session)


def get_location_address(latitude, longitude):
    try:
        geolocator = Nominatim(user_agent="myApp")
        location = geolocator.reverse((latitude, longitude), language='en')

        if location is not None:
            address = location.raw
            address_details = address.get('address', {})
        else:
            address_details = {}

        address_string = ''
        if 'road' in address_details:
            address_string += f"Street/road: {address_details['road']} "
        if 'city' in address_details:
            address_string += f"City: {address_details['city']} "
        if 'country' in address_details:
            address_string += f"Country: {address_details['country']}"

        return address_string
    except Exception as e:  # noqa
        return ""


@app.route('/api/air-quality', methods=['POST'])
def air_quality_api():
    data = request.get_json()  # Get the JSON data from the request
    lat = data.get("lat")  # Extract latitude
    lng = data.get("lng")  # Extract longitude

    if lat is None or lng is None:
        return jsonify({"error": "Latitude and Longitude are required"}), 400

    # Make the Open-Meteo API call
    url = "https://air-quality-api.open-meteo.com/v1/air-quality"
    params = {
        "latitude": lat,
        "longitude": lng,
        "current": ["european_aqi", "pm10", "pm2_5", "carbon_monoxide", "nitrogen_dioxide", "sulphur_dioxide", "ozone",
                    "aerosol_optical_depth", "dust"],
        "hourly": "european_aqi",
        "past_days": 7,
        "forecast_days": 0
    }
    responses = openmeteo.weather_api(url, params=params)

    if not responses:
        return jsonify({"error": "Failed to fetch data from Open-Meteo API"}), 500

    # Process the first response
    response = responses[0]
    current = response.Current()

    hourly = response.Hourly()
    hourly_european_aqi = hourly.Variables(0).ValuesAsNumpy()
    hourly_data = {"date": pd.date_range(start=pd.to_datetime(hourly.Time(), unit="s", utc=True),
                                         end=pd.to_datetime(hourly.TimeEnd(), unit="s", utc=True),
                                         freq=pd.Timedelta(seconds=hourly.Interval()), inclusive="left"),
                   "european_aqi": hourly_european_aqi}
    hourly_dataframe = pd.DataFrame(data=hourly_data)

    air_quality_data = {
        "current_time": current.Time(),
        "european_aqi": round(current.Variables(0).Value(), 1),
        "pm10": round(current.Variables(1).Value(), 1),
        "pm2_5": round(current.Variables(2).Value(), 1),
        "carbon_monoxide": current.Variables(3).Value(),
        "nitrogen_dioxide": round(current.Variables(4).Value(), 1),
        "sulphur_dioxide": round(current.Variables(5).Value(), 1),
        "ozone": current.Variables(6).Value(),
        "aerosol_optical_depth": round(current.Variables(7).Value(), 2),
        "dust": current.Variables(8).Value(),
        "hourly_aqi_data": hourly_dataframe.to_dict(orient="records"),
        "address": get_location_address(lat, lng)
    }

    return jsonify(air_quality_data)


@app.route('/api/detailed-stats', methods=['POST'])
def get_detailed_stats():
    # Get the location data from the POST request
    data = request.get_json()

    lat = data.get('lat')
    lng = data.get('lng')

    if lat is None or lng is None:
        return jsonify({"error": "Latitude and Longitude are required"}), 400

    url = "https://air-quality-api.open-meteo.com/v1/air-quality"
    params = {
        "latitude": lat,
        "longitude": lng,
        "hourly": ["european_aqi", "european_aqi_pm2_5", "european_aqi_pm10",
                   "european_aqi_nitrogen_dioxide",
                   "european_aqi_ozone", "european_aqi_sulphur_dioxide"],
        "past_days": 30,
        "forecast_days": 0
    }
    responses = openmeteo.weather_api(url, params=params)

    if not responses:
        return jsonify({"error": "Failed to fetch data from Open-Meteo API"}), 500

    response = responses[0]
    hourly = response.Hourly()
    hourly_european_aqi = hourly.Variables(0).ValuesAsNumpy()
    hourly_european_aqi_pm2_5 = hourly.Variables(1).ValuesAsNumpy()
    hourly_european_aqi_pm10 = hourly.Variables(2).ValuesAsNumpy()
    hourly_european_aqi_nitrogen_dioxide = hourly.Variables(3).ValuesAsNumpy()
    hourly_european_aqi_ozone = hourly.Variables(4).ValuesAsNumpy()
    hourly_european_aqi_sulphur_dioxide = hourly.Variables(5).ValuesAsNumpy()

    hourly_data = {"date": pd.date_range(
        start=pd.to_datetime(hourly.Time(), unit="s", utc=True),
        end=pd.to_datetime(hourly.TimeEnd(), unit="s", utc=True),
        freq=pd.Timedelta(seconds=hourly.Interval()),
        inclusive="left"
    ),  "european_aqi": hourly_european_aqi,
        "european_aqi_pm2_5": hourly_european_aqi_pm2_5,
        "european_aqi_pm10": hourly_european_aqi_pm10,
        "european_aqi_nitrogen_dioxide": hourly_european_aqi_nitrogen_dioxide,
        "european_aqi_ozone": hourly_european_aqi_ozone,
        "european_aqi_sulphur_dioxide": hourly_european_aqi_sulphur_dioxide}

    hourly_dataframe = pd.DataFrame(data=hourly_data)

    return jsonify({"detailed_data": hourly_dataframe.to_dict(orient="records"),
                    "address": get_location_address(lat, lng)})


if __name__ == '__main__':
    app.run(debug=True)
