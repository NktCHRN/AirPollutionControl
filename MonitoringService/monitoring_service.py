from flask import Flask, jsonify, request
from flask_cors import CORS
import openmeteo_requests
import requests_cache
from retry_requests import retry

app = Flask(__name__)
CORS(app)

# Setup the Open-Meteo API client with cache and retry on error
cache_session = requests_cache.CachedSession('.cache', expire_after=3600)
retry_session = retry(cache_session, retries=5, backoff_factor=0.2)
openmeteo = openmeteo_requests.Client(session=retry_session)


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
                    "aerosol_optical_depth", "dust"]
    }
    responses = openmeteo.weather_api(url, params=params)

    if not responses:
        return jsonify({"error": "Failed to fetch data from Open-Meteo API"}), 500

    # Process the first response
    response = responses[0]
    current = response.Current()

    air_quality_data = {
        "current_time": current.Time(),
        "european_aqi": round(current.Variables(0).Value(), 1),
        "pm10": round(current.Variables(1).Value(), 1),
        "pm2_5": round(current.Variables(2).Value(), 1),
        "carbon_monoxide": current.Variables(3).Value(),
        "nitrogen_dioxide": round(current.Variables(4).Value(), 1),
        "sulphur_dioxide": current.Variables(5).Value(),
        "ozone": current.Variables(6).Value(),
        "aerosol_optical_depth": round(current.Variables(7).Value(), 2),
        "dust": current.Variables(8).Value(),
    }

    return jsonify(air_quality_data)


if __name__ == '__main__':
    app.run(debug=True)
